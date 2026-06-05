using System.Reflection;
using SdmFramework.Service.ViewEngine.ViewObjects;

namespace SdmFramework.Utils;
/// <summary>
/// Represents information about a controller action, including the controller instance,
/// the method to invoke, and related details.
/// </summary>
public class ActionInfo
{
    public object Controller { get; }
    
    public MethodInfo Method { get;}
    public Func<object[], IActionResult> ActionDelegate { get; }
    public CustomParameterInfo[] Parameters { get; }

    public ActionInfo(object controller, MethodInfo originalMethod)
    {
        Controller = controller;
        Method = originalMethod;
        
        Parameters = InitializeParameters(Method.GetParameters());
    }
    /// <summary>
    /// Initializes an array of custom parameter information based on the original parameters of the action method.
    /// </summary>
    /// <param name="originalParameters">The original parameters obtained from the action method.</param>
    /// <returns>An array of <see cref="CustomParameterInfo"/> representing the custom attributes applied to each parameter.</returns>
    private CustomParameterInfo[] InitializeParameters(ParameterInfo[] originalParameters)
    {
        var customParameters = new CustomParameterInfo[originalParameters.Length];

        for (int i = 0; i < originalParameters.Length; i++)
        {
            var customAttribute = GetCustomAttributeName(originalParameters[i]);
            customParameters[i] = new CustomParameterInfo(originalParameters[i], customAttribute);
        }

        return customParameters;
    }
    private Attribute[] GetCustomAttributeName(ParameterInfo parameter)
    {
        return parameter.GetCustomAttributes() as Attribute[];
    }
    
    /// <summary>
    /// Invokes the action with the provided parameters.
    /// </summary>
    /// <param name="parameters">The parameters to pass to the action.</param>
    /// <returns>The result of the action invocation.</returns>
    public object? Invoke(Dictionary<string, string> parameters)
    {
        var convertedparams = MapParameters(parameters);
        return Method.Invoke(Controller, convertedparams);
    }
    /// <summary>
    /// Maps and converts extracted parameters to match the expected parameter types of the action method.
    /// </summary>
    /// <param name="extractedParameters">The extracted parameters from the request.</param>
    /// <returns>An array of objects representing the converted parameters.</returns>
    private object[] MapParameters(Dictionary<string, string> extractedParameters)
    {
        List<object> paramHelperList = new List<object>();
        foreach (var expectedparam in Method.GetParameters())
        {
            var expectedtype = expectedparam.ParameterType;
            
            if(extractedParameters.TryGetValue(expectedparam.Name, out var extractedParam))
            {
                var convertedParam = Convert.ChangeType(extractedParam, expectedtype);
                paramHelperList.Add(convertedParam);
            }
        }
        return paramHelperList.ToArray();
    }
}
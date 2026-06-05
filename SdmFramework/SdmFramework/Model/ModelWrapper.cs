namespace SdmFramework.Model;

using System;
using System.Collections.Generic;
using System.Linq;


public class ModelWrapper
{
    /// <summary>
    /// Wraps an object instance and provides convenient access to its properties.
    /// </summary>
    public object ModelInstance { get; set; }
    private readonly Dictionary<string, object> _propertyValues = new Dictionary<string, object>();

    public ModelWrapper(object modelInstance)
    {
        ModelInstance = modelInstance;
        Type modelType = ModelInstance.GetType();
        if (!(ModelInstance is IEnumerable<object>))
        {
            if (modelType.IsPrimitive)
            {
                _propertyValues["Value"] = ModelInstance;
            }
            
            PopulatePropertyValues();
        }
    }
    /// <summary>
    /// Populates the dictionary with property values from the wrapped model instance.
    /// </summary>
    private void PopulatePropertyValues()
    {
        Type modelType = ModelInstance.GetType();

        foreach (var propertyInfo in modelType.GetProperties())
        {
            
            if (IsEnumerableType(propertyInfo.PropertyType))
            {
                
                var collection = propertyInfo.GetValue(ModelInstance) as IEnumerable<object>;

                
                _propertyValues[propertyInfo.Name] = collection?.ToList();
            }
            else
            {
                
                var value = propertyInfo.GetValue(ModelInstance);

                
                _propertyValues[propertyInfo.Name] = value;
            }
        }
    }

    public object GetPropertyValue(string propertyName)
    {
        if (_propertyValues.TryGetValue(propertyName, out var value))
        {
            return value;
        }
        else
        {
            throw new InvalidOperationException($"Property '{propertyName}' not found on the model.");
        }
    }

    private bool IsEnumerableType(Type type)
    {
        return type != typeof(string) && type.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
    }
}



using System.Text.RegularExpressions;
using SdmFramework.Annotations;
using SdmFramework.Registry.interfaces;

using SdmFramework.Service.HttpHandler;
using SdmFramework.Utils;

namespace SdmFramework.Registry;
/// <summary>
/// Manages the registration and retrieval of routes for different HTTP methods.
/// </summary>
public class RouteRegistry : IRouteRegistry
{
    private readonly RoutePathProcessor _routePathProcessor;
    
    private readonly Dictionary<string, ActionInfo> _getRoutes = new();
    
    private readonly Dictionary<string, ActionInfo> _postRoutes = new();
    
    private readonly Dictionary<string, ActionInfo> _putRoutes = new();
    
    private readonly Dictionary<string, ActionInfo> _deleteRoutes = new();

    public RouteRegistry(RoutePathProcessor routePathProcessor)
    {
        _routePathProcessor = routePathProcessor;
    }

    /// <summary>
    /// Registers a route for a specific HTTP method and associates it with the provided action.
    /// </summary>
    /// <param name="attribute">The HTTP method attribute (HttpGet, HttpPost, HttpPut, HttpDelete).</param>
    /// <param name="routeKey">The unique key representing the route.</param>
    /// <param name="action">The <see cref="ActionInfo"/> representing the action associated with the route.</param>
    /// <exception cref="ArgumentException">Thrown if an ambiguous route is detected.</exception>
    /// <exception cref="NotSupportedException">Thrown if an unsupported HTTP method attribute is provided.</exception>
    public void RegisterRoute(HttpRequest attribute, string routeKey, ActionInfo action)
    {
        switch (attribute)
        {
            case HttpGet: 
                if(ValidateRoute(_getRoutes.Keys.ToList(), routeKey, action)) 
                    throw new ArgumentException($"Ambiguous route: {routeKey}");
                _getRoutes.Add(routeKey, action);
                break;
            
            case HttpPut:
                if(ValidateRoute(_putRoutes.Keys.ToList(), routeKey, action)) 
                    throw new ArgumentException($"Ambiguous route: {routeKey}");
                _putRoutes.Add(routeKey, action);
                break;
            
            case HttpPost:
                if(ValidateRoute(_postRoutes.Keys.ToList(), routeKey, action)) 
                    throw new ArgumentException($"Ambiguous route: {routeKey}");
                _postRoutes.Add(routeKey, action);
                break;
            
            case HttpDelete:
                if(ValidateRoute(_deleteRoutes.Keys.ToList(), routeKey,action)) 
                    throw new ArgumentException($"Ambiguous route: {routeKey}");
                _deleteRoutes.Add(routeKey, action);
                break;
            
            default:
                throw new NotSupportedException($"Unsupported HTTP method attribute: {attribute.GetType().Name}");
        }
    }

    public Dictionary<string, ActionInfo> GetRequestDictionary(string request)
    {
        switch (request)
        {
            case "GET": return _getRoutes;
                
            case "POST": return _postRoutes;
                
            case "PUT": return _putRoutes;
                
            case "DELETE": return _deleteRoutes;
                
        }

        throw new Exception();
    }

    /// <summary>
    /// Validates whether a route is ambiguous and checks if the parameters match between the existing routes and the new route.
    /// </summary>
    /// <param name="existingRoutes">The list of existing routes for the current HTTP method.</param>
    /// <param name="routeKey">The unique key representing the new route.</param>
    /// <param name="action">The <see cref="ActionInfo"/> representing the action associated with the route.</param>
    /// <returns>True if the route is ambiguous or parameters do not match; otherwise, false.</returns>
    private bool ValidateRoute(List<string> existingRoutes, string routeKey, ActionInfo action)
    {
        bool isAmbigious = IsAmbiguousRoute(existingRoutes, routeKey);
        bool isValid = CheckParameters(routeKey, action);

        return isAmbigious && isValid;
    }

    private bool CheckParameters(string routeKey, ActionInfo action)
    {
        var pathVariables = _routePathProcessor.ExtractAllPathVariables(routeKey);
        var parameters = action.Parameters;

        
        for (int i = 0; i < pathVariables.Count; i++)
        {
            var pathVariableName = pathVariables[i];
            var parameterName = parameters[i].OriginalParameter.Name;

            
            if (!string.Equals(pathVariableName, parameterName))
            {
                return false;
            }
        }
        return true;
    }
    private bool IsAmbiguousRoute(List<string> existingRoutes, string routeKey)
    {
        foreach (var existingRoute in existingRoutes)
        {
            if (RoutesAreAmbiguous(existingRoute, routeKey))
            {
                return true;
            }
        }
        return false;
    }

    private bool RoutesAreAmbiguous(string existingRoute, string newRoute)
    {
        string placeholderRoute1 = ReplaceTypesWithPlaceholder(existingRoute);
        string placeholderRoute2 = ReplaceTypesWithPlaceholder(newRoute);

       
        return string.Equals(placeholderRoute1, placeholderRoute2, StringComparison.OrdinalIgnoreCase);
    }

    private string ReplaceTypesWithPlaceholder(string route)
    {
        return Regex.Replace(route, @"\{\w+:\w+\}", "{}");
    }
}
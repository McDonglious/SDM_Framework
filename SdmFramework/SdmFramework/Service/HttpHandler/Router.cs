using SdmFramework.Registry.interfaces;
using SdmFramework.Service.HttpHandler.interfaces;
using SdmFramework.Utils;

namespace SdmFramework.Service.HttpHandler;

public class Router : IRouter
{
    private readonly Resolver _resolver;
    private readonly IRouteRegistry _routeRegistry;

    /// <summary>
    /// Initializes a new instance of the <see cref="Router"/> class.
    /// </summary>
    /// <param name="routeRegistry">The route registry for storing and retrieving route patterns and actions.</param>
    /// <param name="resolver">The resolver for matching request paths with registered route patterns.</param>
    public Router(IRouteRegistry routeRegistry, Resolver resolver)
    {
        _routeRegistry = routeRegistry;
        _resolver = resolver;
    }

    /// <summary>
    /// Resolves and executes the action for the given HTTP request.
    /// </summary>
    /// <param name="requestType">The type of the HTTP request (e.g., GET, POST).</param>
    /// <param name="path">The path of the HTTP request.</param>
    /// <param name="query">The query string of the HTTP request.</param>
    /// <returns>The result of the executed action, or null if no matching route is found.</returns>
    public object? ResolveRoute(string requestType, string path, string query)
    {
        var routes = _routeRegistry.GetRequestDictionary(requestType);

        foreach (var route in routes)
        {
            if (_resolver.Resolve(path, route.Key))
            {
                var parameters = RoutePathHelper.ExtractAllVariables(path, route.Key, query);
                return route.Value.Invoke(parameters);
            }
        }

        return null;
    }
}
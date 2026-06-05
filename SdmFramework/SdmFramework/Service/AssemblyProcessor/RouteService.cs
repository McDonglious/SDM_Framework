using System.Reflection;
using SdmFramework.Annotations;
using SdmFramework.Registry.interfaces;
using SdmFramework.Service.AssemblyProcessor.interfaces;
using SdmFramework.Utils;

namespace SdmFramework.Service.AssemblyProcessor;

/// <summary>
/// Service responsible for scanning and registering Routes in the system.
/// </summary>
public class RouteService : IRouteService
{
    private readonly IAssemblyScanner _assemblyScanner;
    
    private readonly IRouteRegistry _routeRegistry;

    /// <summary>
    /// Initializes a new instance of the <see cref="RouteService"/> class.
    /// </summary>
    /// <param name="assemblyScanner">The assembly scanner used to identify controllers.</param>
    /// <param name="controllerRegistry">The registry for storing registered controllers.</param>
    /// <param name="routeRegistry">The registry for storing registered routes.</param>
    public RouteService(IAssemblyScanner assemblyScanner, IRouteRegistry routeRegistry)
    {
        _assemblyScanner = assemblyScanner;
        _routeRegistry = routeRegistry;
    }

    /// <summary>
    /// Scans the specified <paramref name="type"/> and registers the routes.
    /// </summary>
    /// <param name="type">The type used to determine the assembly for scanning.</param>
    public void RunControllerScan(Type type)
    {
        var controllerTypes = _assemblyScanner.ScanForControllers(type);

        foreach (var controllerType in controllerTypes)
        {
            var key = controllerType.Name;

            Console.WriteLine("key: " + key);
            var controllerInstance = Activator.CreateInstance(controllerType);
            Console.WriteLine("controller instance: " + controllerInstance);

            if (controllerInstance != null)
            {
                var controllerRoute = controllerType.GetCustomAttribute<Controller>()?.Path;

                var methods = controllerType.GetMethods();
                foreach (var method in methods)
                {
                    HttpRequest request = HttpRequest.GetHttpAttribute(method);
                    if (request != null)
                    {
                        string uniqueKey = controllerRoute + request.Path;
                        ActionInfo actionInfo = new ActionInfo(controllerInstance, method);
                        _routeRegistry.RegisterRoute(request, uniqueKey, actionInfo);
                    }
                }
            }
        }
    }
}

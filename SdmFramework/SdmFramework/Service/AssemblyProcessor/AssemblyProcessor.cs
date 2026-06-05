using SdmFramework.Service.AssemblyProcessor.interfaces;

/// <summary>
/// An implementation of <see cref="IAssemblyProcessor"/> that processes types for controller scanning.
/// </summary>
public class AssemblyProcessor : IAssemblyProcessor
{
    private readonly IRouteService _routeService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyProcessor"/> class.
    /// </summary>
    /// <param name="routeService">The service responsible for scanning and processing controllers.</param>
    public AssemblyProcessor(IRouteService routeService)
    {
        _routeService = routeService ?? throw new ArgumentNullException(nameof(routeService));
    }

    /// <summary>
    /// Processes the specified type for controller scanning.
    /// </summary>
    /// <param name="type">The type to be processed.</param>
    public void Process(Type type)
    {
        _routeService.RunControllerScan(type);
    }
}
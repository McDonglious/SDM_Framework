
using SdmFramework.Registry;
using SdmFramework.Registry.interfaces;
using SdmFramework.Service.AssemblyProcessor;
using SdmFramework.Service.AssemblyProcessor.interfaces;
using SdmFramework.Service.HttpHandler;
using SdmFramework.Service.HttpHandler.interfaces;
using SdmFramework.Service.HttpProcessor.interfaces;
using SdmFramework.Service.ViewEngine;

namespace SdmFramework;
/// <summary>
/// Represents the application entry point for the SdmFramework, responsible for initializing and running the framework.
/// </summary>
public class SdmFrameWorkApplication
{
    private SdmFramework _sdmFramework;
    /// <summary>
    /// Gets or sets the base URL that the framework will listen to
    /// </summary>
    public string baseUrl { get; set; }
    public SdmFrameWorkApplication(string baseUrl)
    {
        this.baseUrl = baseUrl;
    }

    
    
    private void InitializeFramework()
    {
        RoutePathProcessor routePathProcessor = new RoutePathProcessor();
        IRouteRegistry routeRegistry = new RouteRegistry(routePathProcessor);
        
        IAssemblyScanner assemblyScanner = new AssemblyScanner();
        RouteService routeService = new RouteService(assemblyScanner, routeRegistry);
        
        IAssemblyProcessor assemblyProcessor = new AssemblyProcessor(routeService);
        
        Resolver resolver = new Resolver();
        IRouter router = new Router(routeRegistry, resolver);

        ParserService parserService = new ParserService();
        ViewService viewService = new ViewService(parserService);
        IHttpHandler httpHandler = new HttpHandler(new[] { baseUrl }, router, viewService);
        
        _sdmFramework = new SdmFramework(assemblyProcessor, httpHandler);
    }
    public void Run(Type type)
    {
        InitializeFramework();
        _sdmFramework.Run(type);
    }
}
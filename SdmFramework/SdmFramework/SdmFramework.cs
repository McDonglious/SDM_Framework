using SdmFramework.Service.AssemblyProcessor.interfaces;
using SdmFramework.Service.HttpProcessor.interfaces;

namespace SdmFramework;
public class SdmFramework
{
    static ManualResetEventSlim _waitHandle = new ManualResetEventSlim(false);
    private readonly IAssemblyProcessor _assemblyProcessor;
    private readonly IHttpHandler _httpHandler;

    public SdmFramework(IAssemblyProcessor assemblyProcessor, IHttpHandler httpHandler)
    {
        _assemblyProcessor = assemblyProcessor;
        _httpHandler = httpHandler;
    }


    public void Run(Type type)
    {
        _assemblyProcessor.Process(type);
        _httpHandler.Start();
        
        _waitHandle.Wait();
        
        _httpHandler.Stop();
    }
}
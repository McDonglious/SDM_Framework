namespace SdmFramework.Service.AssemblyProcessor.interfaces;

public interface IAssemblyScanner
{
     IEnumerable<Type> ScanForControllers(Type type);

}
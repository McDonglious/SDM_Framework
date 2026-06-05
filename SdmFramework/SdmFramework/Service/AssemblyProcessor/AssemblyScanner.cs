using System.Reflection;
using SdmFramework.Annotations;
using SdmFramework.Service.AssemblyProcessor.interfaces;

namespace SdmFramework.Service.AssemblyProcessor;


/// <summary>
/// An implementation of <see cref="IAssemblyScanner"/> that scans types for controllers.
/// </summary>
public class AssemblyScanner : IAssemblyScanner
{
    /// <summary>
    /// Scans the specified <paramref name="type"/> and returns a collection of types marked with the <see cref="Controller"/> attribute.
    /// </summary>
    /// <param name="type">The type used to determine the assembly for scanning.</param>
    /// <returns>A collection of types representing controllers.</returns>
    public IEnumerable<Type> ScanForControllers(Type type)
    { 
        return type.Assembly
            .GetTypes()
            .Where(t => t.GetCustomAttributes(typeof(Controller)).Any());
    }
}


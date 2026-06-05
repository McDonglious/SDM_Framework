namespace SdmFramework.Annotations;
/// <summary>
/// Marks a class as a controller with a specified path.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class Controller : Attribute
{
    public string Path { get; }

    public Controller(string path)
    {
        Path = path;
    }
}
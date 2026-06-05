using System.Reflection;

namespace SdmFramework.Annotations;
/// <summary>
/// Marks a method as an HTTP request handler with a specified path.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class HttpRequest : Attribute
{
    public string Path { get; }

    public HttpRequest(string path)
    {
        Path = path;
    }
    
    public static HttpRequest GetHttpAttribute(MethodInfo method)
    {
        return method.GetCustomAttributes().OfType<HttpRequest>().FirstOrDefault();
    }
}
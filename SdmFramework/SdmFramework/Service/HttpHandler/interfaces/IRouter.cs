using SdmFramework.Model;

namespace SdmFramework.Service.HttpHandler.interfaces;

public interface IRouter
{
    object? ResolveRoute(string request, string path, string query);
}
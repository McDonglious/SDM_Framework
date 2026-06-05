using SdmFramework.Annotations;
using SdmFramework.Utils;

namespace SdmFramework.Registry.interfaces;

public interface IRouteRegistry
{
    void RegisterRoute(HttpRequest attribute, string routeKey, ActionInfo action);

    Dictionary<string, ActionInfo> GetRequestDictionary(string request);
}
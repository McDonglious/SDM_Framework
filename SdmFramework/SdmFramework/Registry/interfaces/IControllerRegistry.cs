namespace SdmFramework.Registry.interfaces;

public interface IControllerRegistry
{
    public void RegisterController(string key, object controllerInstance);
    public object? GetController(string key);
    public Dictionary<string, object> GetAllControllers();

}
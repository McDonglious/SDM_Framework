namespace SdmFramework.Service.ViewEngine.ViewObjects;

/// <summary>
/// Represents the result of an action as a view with a specified path and model.
/// </summary>
public class View : IActionResult
{
    public string Path { get; }
    public object Model { get; }
    public View(string path, object model)
    {
        Path = path;
        Model = model;
    }

    public string execute()
    {
        throw new NotImplementedException();
    }
}
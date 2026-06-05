namespace SdmFramework.Service.ViewEngine.ViewObjects;

/// <summary>
/// Represents the result of an action as a view with a specified name and model.
/// The framework resolves the view file path by convention.
/// </summary>
public class View : IActionResult
{
    public string Name { get; }
    public object Model { get; }
    
    public View(string name, object model)
    {
        Name = name;
        Model = model;
    }

    public string execute()
    {
        throw new NotImplementedException();
    }
}
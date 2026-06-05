
using SdmFramework.Service.ViewEngine.ViewObjects;

namespace SdmFramework.Service.ViewEngine;
/// <summary>
/// Service for processing views and generating HTML content.
/// </summary>
public class ViewService
{
    private readonly ParserService _parserService;

    public ViewService(ParserService parserService)
    {
        _parserService = parserService;
    }
    /// <summary>
    /// Processes a view result and generates HTML content.
    /// </summary>
    /// <param name="actionResult">The action result representing the view.</param>
    /// <returns>The HTML content generated from the view.</returns>
    public string ProcessView(IActionResult actionResult)
    {
        if (actionResult is View view)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string fullPath = Path.Combine(basePath, "View", view.Name + ".scl");
        
            string code = File.ReadAllText(fullPath);
            string html = _parserService.ParseAndVisit(code, view.Model);
            return html;  
        }

        throw new NotImplementedException();
    }
}
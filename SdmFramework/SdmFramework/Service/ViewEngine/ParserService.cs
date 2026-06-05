using Antlr4.Runtime;
using SdmFramework.Service.ViewEngine.Compiler.Visitor;

namespace SdmFramework.Service.ViewEngine;
/// <summary>
/// Service for parsing and visiting Razor code using Antlr4.
/// </summary>
public class ParserService
{
    /// <summary>
    /// Parses and visits Razor code with the provided model.
    /// </summary>
    /// <param name="code">The Razor code to parse and visit.</param>
    /// <param name="model">The model object to use during the parsing and visiting process.</param>
    /// <returns>A string representing the result of parsing and visiting the Razor code in HTML form.</returns>
    public string ParseAndVisit(string code, object model)
    {
        AntlrInputStream inputStream = new AntlrInputStream(code);
        BasicRazorLexer basicRazorLexer = new BasicRazorLexer(inputStream);
        CommonTokenStream commonTokenStream = new CommonTokenStream(basicRazorLexer);
        BasicRazorParser basicRazorParser = new BasicRazorParser(commonTokenStream);
        var codeContext = basicRazorParser.razorCode();
        var visitor = new SdmVisitor(model);
        var htmlResult = visitor.Visit(codeContext);
        return htmlResult as string ?? "";
    }
}
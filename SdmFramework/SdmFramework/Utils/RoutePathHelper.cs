using System.Text.RegularExpressions;

namespace SdmFramework.Utils;
/// <summary>
/// Helper class for processing route paths and extracting variables from paths and queries.
/// </summary>
public class RoutePathHelper
{
    private static string[] Segmenter(string route)
    {
        
        return route.Trim('/').Split("/");
    }
    /// <summary>
    /// Extracts all variables (path and query) from the given path and route.
    /// </summary>
    /// <param name="path">The actual path from the request URL.</param>
    /// <param name="route">The route pattern containing path variables.</param>
    /// <param name="query">The query string from the request URL.</param>
    /// <returns>A dictionary containing all extracted variables.</returns>
    public static Dictionary<string, string> ExtractAllVariables(string path, string route, string query)
    {
        
        var queryParameters = ExtractQueryParameters(query);
        var pathVariables = ExtractPathVariables(route, path);
        
        var allParameters = queryParameters.Union(pathVariables)
            .ToDictionary(pair => pair.Key, pair => pair.Value);

        return allParameters;
    }
    
    public static bool IsSegmentPathVariable(string segment)
    {
        return segment.StartsWith("{") && segment.EndsWith("}");
    }
    public static bool ContainsQueryParameter(string path)
    {
        return path.Contains("?");
    }
    /// <summary>
    /// Extracts query parameters from the given path.
    /// </summary>
    /// <param name="path">The path containing a query string.</param>
    /// <returns>A dictionary containing extracted query parameters.</returns>
    public static Dictionary<string, string> ExtractQueryParameters(string path)
    {
        var parameters = new Dictionary<string, string>();
        if (ContainsQueryParameter(path))
        {
            string[] splitPath = path.Split("?");
            
            string[] splitQuery = splitPath[1].Split("&");

            foreach (var subQuery in splitQuery)
            {
                string[] splitSubQuery = subQuery.Split("=");
                if (splitSubQuery.Length == 2)
                {
                    parameters[splitSubQuery[0]] = splitSubQuery[1];
                }
            }
        }
        return parameters;
    }
    /// <summary>
    /// Extracts path variables from the given route and path.
    /// </summary>
    /// <param name="route">The route pattern containing path variables.</param>
    /// <param name="path">The actual path from the request URL.</param>
    /// <returns>A dictionary containing extracted path variables.</returns>
    public static Dictionary<string, string> ExtractPathVariables(string route, string path)
    {
        var pathVariables = new Dictionary<string, string>();
        var routeSegments = Segmenter(route);
        var pathSegments = Segmenter(path);


        for (int i = 0; i < routeSegments.Length; i++)
        {
            if (IsSegmentPathVariable(routeSegments[i]))
            {
                var variableName = ExtractPathVariableName(routeSegments[i]);
                var variableValue = pathSegments[i];
                pathVariables.Add(variableName, variableValue);
            }
        }
        return pathVariables;
    }
    
    private static string ExtractPathVariableName(string routeSegment)
    {
        var match = Regex.Match(routeSegment, @"{\s*([A-Za-z_][A-Za-z0-9_]*)\s*}");

        if (match.Success)
        {
            return match.Groups[1].Value;
        }

        throw new ArgumentException();
    }
}
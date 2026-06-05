using System.Text.RegularExpressions;

namespace SdmFramework.Service.HttpHandler
{
    public class RoutePathProcessor
    {
        /// <summary>
        /// Splits a route into segments based on '/'.
        /// </summary>
        /// <param name="route">The route pattern to split.</param>
        /// <returns>An array of segments extracted from the route pattern.</returns>
        private static string[] Segmenter(string route)
        {
            // Split the route based on '/'
            return route.Trim('/').Split("/");
        }

        /// <summary>
        /// Checks if a segment represents a path variable.
        /// </summary>
        /// <param name="segment">The segment to check.</param>
        /// <returns>True if the segment is a path variable; otherwise, false.</returns>
        private bool IsPathVariable(string segment)
        {
            return segment.StartsWith("{") && segment.EndsWith("}");
        }

        /// <summary>
        /// Extracts the name of a path variable from a route segment.
        /// </summary>
        /// <param name="routeSegment">The route segment containing a path variable.</param>
        /// <returns>The name of the extracted path variable.</returns>
        /// <exception cref="ArgumentException">Thrown if the extraction fails.</exception>
        private string ExtractPathVariableName(string routeSegment)
        {
            var match = Regex.Match(routeSegment, @"{\s*([A-Za-z_][A-Za-z0-9_]*)\s*}");

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            throw new ArgumentException("Invalid route segment: " + routeSegment);
        }

        /// <summary>
        /// Extracts all path variable names from a route pattern.
        /// </summary>
        /// <param name="route">The route pattern to process.</param>
        /// <returns>A list of extracted path variable names.</returns>
        public List<string> ExtractAllPathVariables(string route)
        {
            var pathVariables = new List<string>();
            var segments = Segmenter(route);

            foreach (var segment in segments)
            {
                if (IsPathVariable(segment))
                {
                    var paramName = ExtractPathVariableName(segment);
                    pathVariables.Add(paramName);
                }
            }

            return pathVariables;
        }
    }
}
    

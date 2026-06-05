namespace SdmFramework.Service.HttpHandler;

public class Resolver
{
    /// <summary>
        /// Resolves whether the provided path matches the specified route pattern.
        /// </summary>
        /// <param name="path">The path to be resolved.</param>
        /// <param name="route">The route pattern to compare against.</param>
        /// <returns>True if the path matches the route pattern; otherwise, false.</returns>
        public bool Resolve(string path, string route)
        {
            
            path = PathCleaner(path);
            
            var pathSegments = Segmenter(path);
            var routeSegments = Segmenter(route);

            
            if (pathSegments.Length == routeSegments.Length)
            {
                
                return MatchSegments(pathSegments, routeSegments);
            }

            return false;
        }

        /// <summary>
        /// Matches path segments against route segments.
        /// </summary>
        /// <param name="pathSegments">Segments extracted from the path.</param>
        /// <param name="routeSegments">Segments extracted from the route pattern.</param>
        /// <returns>True if segments match; otherwise, false.</returns>
        private bool MatchSegments(string[] pathSegments, string[] routeSegments)
        {
            for (int i = 0; i < routeSegments.Length; i++)
            {
                if (IsPathVariable(routeSegments[i]))
                {
                    
                }
                else if (!string.Equals(routeSegments[i], pathSegments[i], StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            return true;
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
        /// Cleans the path by removing query parameters if present.
        /// </summary>
        /// <param name="path">The path to clean.</param>
        /// <returns>The cleaned path without query parameters.</returns>
        private string PathCleaner(string path)
        {
            // Check if the path contains query parameters
            if (ContainsQueryParameter(path))
            {
                // Split the path to remove query parameters
                string[] splitPath = path.Split("?");
                return splitPath[0];
            }

            return path;
        }

        /// <summary>
        /// Checks if the path contains query parameters.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns>True if the path contains query parameters; otherwise, false.</returns>
        private bool ContainsQueryParameter(string path)
        {
            return path.Contains('?');
        }

        /// <summary>
        /// Splits the route into segments.
        /// </summary>
        /// <param name="route">The route pattern to split.</param>
        /// <returns>An array of segments extracted from the route pattern.</returns>
        private static string[] Segmenter(string route)
        {
            return route.Trim('/').Split("/");
        }
}
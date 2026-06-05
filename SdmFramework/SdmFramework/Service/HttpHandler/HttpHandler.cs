using System.Net;
using System.Text;
using SdmFramework.Service.HttpHandler.interfaces;
using SdmFramework.Service.HttpProcessor.interfaces;
using SdmFramework.Service.ViewEngine;
using SdmFramework.Service.ViewEngine.ViewObjects;

namespace SdmFramework.Service.HttpHandler
{
    /// <summary>
    /// Handles incoming HTTP requests and processes corresponding actions.
    /// </summary>
    public class HttpHandler : IHttpHandler
    {
        private readonly HttpListener _listener;
        private readonly IRouter _router;
        private readonly ViewService _viewService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHandler"/> class.
        /// </summary>
        /// <param name="prefixes">The prefixes on which the HTTP handler will listen.</param>
        /// <param name="router">The router responsible for resolving routes.</param>
        /// <param name="viewService">The service for processing views.</param>
        public HttpHandler(string[] prefixes, IRouter router, ViewService viewService)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException("HTTP Listener is not supported on this platform.");

            _listener = new HttpListener();
            _router = router;
            _viewService = viewService;

            foreach (var prefix in prefixes)
            {
                _listener.Prefixes.Add(prefix);
            }
        }

        /// <summary>
        /// Starts the HTTP handler to listen for incoming requests.
        /// </summary>
        public void Start()
        {
            _listener.Start();
            Console.WriteLine("HTTP Server started.");
            Task.Run(() => ProcessRequests());
            // Start handling requests in a separate thread
        }

        /// <summary>
        /// Stops the HTTP handler.
        /// </summary>
        public void Stop()
        {
            _listener.Stop();
            Console.WriteLine("HTTP Server stopped.");
        }
        /// <summary>
        /// Handles the generation of an HTTP response based on the provided <see cref="IActionResult"/>.
        /// </summary>
        /// <param name="response">The HTTP response object to be populated.</param>
        /// <param name="actionResult">The action result representing the response content.</param>
        private async Task HandleActionResult(HttpListenerResponse response, IActionResult actionResult)
        {
            
            response.ContentType = "text/html";
            response.StatusCode = (int)HttpStatusCode.OK;

            
            string content = _viewService.ProcessView(actionResult);

            
            byte[] buffer = Encoding.UTF8.GetBytes(content);

            
            response.ContentLength64 = buffer.Length;

            
            using (Stream outputStream = response.OutputStream)
            {
                await outputStream.WriteAsync(buffer, 0, buffer.Length);
            }
        }
        /// <summary>
        /// Asynchronously processes incoming HTTP requests and handles corresponding actions.
        /// </summary>
        private async Task ProcessRequests()
        {
            while (_listener.IsListening)
            {
                try
                {
                    var context = await _listener.GetContextAsync();

                    var request = context.Request;
                    if (request != null)
                    {
                        
                        var requestType = context.Request.HttpMethod;

                        
                        var path = request.Url.LocalPath;
                        var query = request.Url.Query;

                        
                        var result = _router.ResolveRoute(requestType, path, query);
                        if (result is IActionResult actionResult) await HandleActionResult(context.Response, actionResult);
                        

                        
                        context.Response.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing request: {ex.Message}");
                    Console.WriteLine($"stack track: {ex.StackTrace}");
                }
            }
        }
    }
}

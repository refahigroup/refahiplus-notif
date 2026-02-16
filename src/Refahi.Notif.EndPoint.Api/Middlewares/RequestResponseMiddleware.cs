namespace Refahi.Notif.EndPoint.Api.Middlewares
{

    public class RequestResponseLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger;

        public RequestResponseLoggerMiddleware(RequestDelegate next, Serilog.ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();

            var request = await GenerateRequestInformation(httpContext);

            //var originalResponseBody = httpContext.Response.Body;

            //using var newResponseBody = new MemoryStream();
            //httpContext.Response.Body = newResponseBody;

            await _next(httpContext);

            //newResponseBody.Seek(0, SeekOrigin.Begin);

            //var response = await GenerateResponseInformation(httpContext);

            //newResponseBody.Seek(0, SeekOrigin.Begin);
            //await newResponseBody.CopyToAsync(originalResponseBody);

            //if (httpContext.Response.StatusCode != 200)
            //_logger.Error($"{request}\n{response}");
        }

        private async Task<string> GenerateResponseInformation(HttpContext httpContext)
        {
            var responseBodyText = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();

            return $"HTTP response information:\n" +
                    $"\tStatusCode: {httpContext.Response.StatusCode}\n" +
                    $"\tContentType: {httpContext.Response.ContentType}\n" +
                    $"\tHeaders: {FormatHeaders(httpContext.Response.Headers)}\n" +
                    $"\tBody: {responseBodyText}";

        }
        private async Task<string> GenerateRequestInformation(HttpContext httpContext)
        {
            return $"HTTP request information:\n" +
                      $"\tMethod: {httpContext.Request.Method}\n" +
                      $"\tPath: {httpContext.Request.Path}\n" +
                      $"\tQueryString: {httpContext.Request.QueryString}\n" +
                      $"\tHeaders: {FormatHeaders(httpContext.Request.Headers)}\n" +
                      $"\tSchema: {httpContext.Request.Scheme}\n" +
                      $"\tHost: {httpContext.Request.Host}\n" +
                      $"\tBody: {await ReadBodyFromRequest(httpContext.Request)}";
        }
        private static string FormatHeaders(IHeaderDictionary headers) => string.Join(", ", headers.Select(kvp => $"{{{kvp.Key}: {string.Join(", ", kvp.Value)}}}"));
        private static async Task<string> ReadBodyFromRequest(HttpRequest request)
        {
            using var streamReader = new StreamReader(request.Body, encoding: System.Text.Encoding.UTF8);
            var requestBody = await streamReader.ReadToEndAsync();

            // Reset the request's body stream position for next middleware in the pipeline.
            request.Body.Position = 0;
            return requestBody;
        }
    }
}
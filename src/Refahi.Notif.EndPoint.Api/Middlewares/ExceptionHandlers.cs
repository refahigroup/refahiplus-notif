using Refahi.Notif.Domain.Core.Exceptions;
using System.Text.Json;

namespace Refahi.Notif.EndPoint.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this.next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();

            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var requestBody = await GenerateRequestInformation(context);

            _logger.LogError($"{ex.Message} , Request :{requestBody}");

            var code = 500;
            object response;
            context.Response.ContentType = "application/json";

            switch (ex)
            {
                case JsonException jsonEx:
                    code = 400;
                    response = new
                    {
                        statusCode = 400,
                        message = "Invalid JSON format",
                        errors = new[] { "The request body contains invalid JSON. Please check your syntax." }
                    };
                    break;

                case BussinessException businessEx:
                    code = 400;
                    response = new
                    {
                        statusCode = 400,
                        message = "Business validation failed",
                        errors = businessEx.Message.Split(',')
                    };
                    break;

                default:
                    response = new
                    {
                        statusCode = 500,
                        message = Errors.UnHandledException,
                        errors = new[] { "An unexpected error occurred. Please try again later." }
                    };
                    break;
            }

            context.Response.StatusCode = code;
            await context.Response.WriteAsJsonAsync(response);
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
            request.Body.Seek(0, SeekOrigin.Begin);

            using var streamReader = new StreamReader(request.Body, encoding: System.Text.Encoding.UTF8);
            var requestBody = await streamReader.ReadToEndAsync();

            // Reset the request's body stream position for next middleware in the pipeline.
            return requestBody;
        }
    }

}


namespace Student.RESTAPI.Middleware 
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(
            RequestDelegate next,
            ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log the request
            var request = await FormatRequest(context.Request);
            _logger.LogInformation($"Request: {request}");

            // Copy the original response body stream
            var originalBodyStream = context.Response.Body;

            // Create a new memory stream to capture the response
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                // Call the next middleware in the pipeline
                await _next(context);

                // Log the response
                var response = await FormatResponse(context.Response);
                _logger.LogInformation($"Response: {response}");

                // Copy the captured response back to the original stream
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering(); // Allows reading the request body multiple times

            using (var reader = new StreamReader(request.Body, leaveOpen: true))
            {
                var body = await reader.ReadToEndAsync();
                request.Body.Position = 0; // Reset for further processing

                return $"{request.Method} {request.Path}{request.QueryString} | Headers: {FormatHeaders(request.Headers)} | Body: {body}";
            }
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin); // Reset for the client

            return $"Status: {response.StatusCode} | Headers: {FormatHeaders(response.Headers)} | Body: {body}";
        }

        private string FormatHeaders(IHeaderDictionary headers)
        {
            var headerString = new System.Text.StringBuilder();
            foreach (var (key, value) in headers)
            {
                headerString.Append($"{key}:{value} ");
            }
            return headerString.ToString();
        }
    }
}
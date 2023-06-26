using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;

namespace SimpleEcommerce.Main.Middlewares
{
    internal sealed class ErrorHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        public ErrorHandlerMiddleware
        (
            ILogger<ErrorHandlerMiddleware> logger
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError($"[Exception]: {exception}");

                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var exceptionObject = new { error = exception.Message };
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var exceptionSerialized = JsonConvert.SerializeObject(new
            {
                success = false,
                response = exceptionObject
            }, jsonSerializerSettings);

            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await context.Response.WriteAsync(exceptionSerialized);
            }
        }
    }
}

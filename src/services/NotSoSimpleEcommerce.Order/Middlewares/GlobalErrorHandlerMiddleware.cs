using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NotSoSimpleEcommerce.Order.Middlewares
{
    internal sealed class GlobalErrorHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalErrorHandlerMiddleware> _logger;
        public GlobalErrorHandlerMiddleware
        (
            ILogger<GlobalErrorHandlerMiddleware> logger
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
                _logger.LogError("[Exception]: {exception}", exception);
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

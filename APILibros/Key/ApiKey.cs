using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APILibros.Key
{
    public class ApiKey : Attribute, IAsyncActionFilter
    {
        private const string HeaderName = "Api-Key";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = configuration["ApiKey"];

            if (!context.HttpContext.Request.Headers.TryGetValue(HeaderName, out var extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Debes introducir una API Key."
                };
                return;
            }
            if (string.IsNullOrWhiteSpace(apiKey) || extractedApiKey != apiKey)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "API Key no válida."
                };
                return;
            }
            await next();
        }
    }
}

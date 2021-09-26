using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace TodoApi.Attributes
{
    public class APIKeyAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("APIKey", out StringValues strAPIKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "No API key."
                };
                return;
            }

            IConfiguration appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            string apiKey = appSettings.GetValue<string>("APIKey");

            if (!apiKey.Equals(strAPIKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Invalid API key."
                };
                return;
            }

            await next();
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;
using System.Runtime.CompilerServices;

namespace ETicaretAPI.API.Extensions
{
    static public class ConfigureExcaptionHandlerExtension
    {
        public static void ConfigureExceptionHandler<T>(this WebApplication application, ILogger<T> logger)
        {
            application.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                  var contextFeature=  context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    {
                        logger.LogError(contextFeature.Error.Message);
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                           StatusCode = context.Response.StatusCode,
                           Message = contextFeature.Error.Message,
                           Title = "hata alındı!"

                        })); 
                    }
                });
            });
        }
    }
}

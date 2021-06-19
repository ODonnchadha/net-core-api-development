using HotelListing.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace HotelListing.Configurations
{
    public static class ExceptionHandlerConfiguration
    {
        public static void AddGlobalErrorHandling(this IApplicationBuilder builder)
        {
            builder.UseExceptionHandler(errorHandler =>
            {
                errorHandler.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (null != contextFeature)
                    {
                        Log.Error($"Error {contextFeature.Error}");

                        await context.Response.WriteAsync(new Error
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error.",

                        }.ToString());
                    }
                });
            });
        }
    }
}

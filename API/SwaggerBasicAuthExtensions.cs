using System;
using Microsoft.AspNetCore.Builder;

namespace API
{
    public static class SwaggerBasicAuthExtensions
    {
        public static void UseSwaggerBasicAuth(
            this IApplicationBuilder app, 
            Action<SwaggerBasicAuthConfiguration> setupAction = null)
        {
            var config = new SwaggerBasicAuthConfiguration
            {
                SwaggerPath = "/swagger",
                Username = "swagger",
                Password = "swagger"
            };

            setupAction?.Invoke(config);

            app.UseMiddleware<SwaggerBasicAuthMiddleware>(config);
        }
    }
}

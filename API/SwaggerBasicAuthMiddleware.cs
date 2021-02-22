using System;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace API
{
    public class SwaggerBasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly SwaggerBasicAuthConfiguration _configuration;

        public SwaggerBasicAuthMiddleware(
            RequestDelegate next,
            SwaggerBasicAuthConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value;

            if (!path.StartsWith(_configuration.SwaggerPath))
            {
                await _next.Invoke(context);
                return;
            }

            var authHeader = context.Request.Headers["Authorization"];

            if (authHeader.Count == 0)
            {
                CreateUnauthorizedResponse(context);
                return;
            }

            var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);

            if (authHeaderValue == null)
            {
                CreateUnauthorizedResponse(context);
                return;
            }

            if (!authHeaderValue.Scheme.Equals(AuthenticationSchemes.Basic.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                CreateUnauthorizedResponse(context);
                return;
            }

            var fromBase64String = Convert.FromBase64String(authHeaderValue.Parameter);

            var credentials = Encoding.UTF8.GetString(fromBase64String).Split(':', 2);

            if (credentials.Length != 2)
            {
                CreateUnauthorizedResponse(context);
                return;
            }

            var username = credentials.FirstOrDefault();
            var password = credentials.LastOrDefault();

            if (username != _configuration.Username || password != _configuration.Password)
            {
                CreateUnauthorizedResponse(context);
                return;
            }

            await _next(context);
        }

        private void CreateUnauthorizedResponse(HttpContext context)
        {
            context.Response.Headers["WWW-Authenticate"] = AuthenticationSchemes.Basic.ToString();
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}

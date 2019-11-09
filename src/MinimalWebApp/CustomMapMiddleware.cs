using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;


namespace MinimalWebApp
{
    public class CustomMapMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly RequestDelegate _next;


        public CustomMapMiddleware(RequestDelegate next, RequestDelegate requestDelegate)
        {
            _next = next;
            _requestDelegate = requestDelegate;
        }


        public async Task Invoke(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof (context));
            var path = context.Request.Path;
            var pathBase = context.Request.PathBase;
            try
            {
                await _requestDelegate(context);
            }
            finally
            {
                context.Request.PathBase = pathBase;
                context.Request.Path = path;
            }
            path = new PathString();
            pathBase = new PathString();
        }
    }
}
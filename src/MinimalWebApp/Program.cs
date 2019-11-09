using System;
using System.IO;
using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;


namespace MinimalWebApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .Configure(app => app
                    .MapAll(EchoHandler)
                )
                .Build();

            host.Run();
        }


        private static void EchoHandler(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var text = string.Empty;

                using (var stream = new StreamReader(context.Request.Body))
                {
                    var content = await stream.ReadToEndAsync();
                    Console.WriteLine($"{context.Request.Method}:{context.Request.Path.Value.Trim('/')}");
                    text = JsonConvert.SerializeObject(new
                    {
                        StatusCode = context.Response.StatusCode.ToString(),
                        PathBase = context.Request.PathBase.Value.Trim('/'),
                        Path = context.Request.Path.Value.Trim('/'),
                        context.Request.Method,
                        context.Request.Scheme,
                        context.Request.ContentType,
                        context.Request.ContentLength,
                        Content = content,
                        QueryString = context.Request.QueryString.ToString(),
                        Query = context.Request.Query
                            .ToDictionary(
                                item => item.Key,
                                item => item.Value,
                                StringComparer.OrdinalIgnoreCase)
                    });
                }

                await context.Response.WriteAsync(text);
            });
        }
    }
}

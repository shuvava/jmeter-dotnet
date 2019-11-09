using System;

using Microsoft.AspNetCore.Builder;


namespace MinimalWebApp
{
    public static class MapExtensions
    {
        public static IApplicationBuilder MapAll(
            this IApplicationBuilder app,
            Action<IApplicationBuilder> configuration)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var applicationBuilder = app.New();
            configuration(applicationBuilder);
            var requestDelegate = applicationBuilder.Build();

            return app.Use(next => new CustomMapMiddleware(next, requestDelegate).Invoke);
        }
    }
}
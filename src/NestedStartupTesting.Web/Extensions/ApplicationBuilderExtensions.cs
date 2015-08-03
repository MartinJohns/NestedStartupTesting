using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Builder.Extensions;
using Microsoft.AspNet.Builder.Internal;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using System;

namespace NestedStartupTesting.Web.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder IsolatedMap(
            this IApplicationBuilder app,
            PathString pathMatch,
            Action<IApplicationBuilder> configuration,
            Action<IServiceCollection> serviceConfiguration,
            IServiceCollection services)
        {
            if (pathMatch.HasValue && pathMatch.Value.EndsWith("/", StringComparison.Ordinal))
            {
                throw new ArgumentException("The path must not end with a '/'", nameof(pathMatch));
            }

            serviceConfiguration(services);
            var serviceProvider = services.BuildServiceProvider();

            var builder = new ApplicationBuilder(null);
            builder.ApplicationServices = serviceProvider;

            builder.Use(async (context, next) =>
            {
                var priorApplicationServices = context.ApplicationServices;
                var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();

                try
                {
                    using (var scope = scopeFactory.CreateScope())
                    {
                        context.ApplicationServices = serviceProvider;
                        context.RequestServices = scope.ServiceProvider;

                        await next();
                    }
                }
                finally
                {
                    context.RequestServices = null;
                    context.ApplicationServices = priorApplicationServices;
                }
            });

            configuration(builder);

            var branch = builder.Build();
            var options = new MapOptions
            {
                Branch = branch,
                PathMatch = pathMatch
            };

            return app.Use(next => new MapMiddleware(next, options).Invoke);
        }
    }
}

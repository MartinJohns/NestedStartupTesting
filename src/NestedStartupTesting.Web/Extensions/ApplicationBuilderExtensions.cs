using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Builder.Extensions;
using Microsoft.AspNet.Builder.Internal;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Hosting.Startup;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Collections.Generic;

namespace NestedStartupTesting.Web.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder IsolatedMap<T>(
            this IApplicationBuilder app,
            PathString pathMatch,
            IServiceCollection services)
        {
            var startupLoader = app.ApplicationServices.GetRequiredService<IStartupLoader>();
            var startupMethods = startupLoader.LoadMethods(typeof(T), new List<string>());

            return app.IsolatedMap(
                pathMatch,
                startupMethods.ConfigureDelegate,
                startupMethods.ConfigureServicesDelegate,
                services);
        }

        public static IApplicationBuilder IsolatedMap(
            this IApplicationBuilder app,
            PathString pathMatch,
            Action<IApplicationBuilder> configuration,
            Func<IServiceCollection, IServiceProvider> serviceConfiguration,
            IServiceCollection services)
        {
            if (pathMatch.HasValue && pathMatch.Value.EndsWith("/", StringComparison.Ordinal))
            {
                throw new ArgumentException("The path must not end with a '/'", nameof(pathMatch));
            }
            
            var serviceProvider = serviceConfiguration(services);
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

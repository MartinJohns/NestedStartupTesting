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
            var builder = new ApplicationBuilder(null);
            builder.ApplicationServices = services.BuildServiceProvider();

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

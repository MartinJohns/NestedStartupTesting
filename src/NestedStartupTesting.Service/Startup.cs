using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using System;
using System.Reflection;

namespace NestedStartupTesting.Service
{
    public class Startup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddControllersAsServices(new[] { typeof(Startup).GetTypeInfo().Assembly });

            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Sample}/{action=Index}/{id?}");
            });

            app.Run(async context =>
            {
                const string payload = "404";
                context.Response.ContentLength = payload.Length;
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(payload);
            });
        }
    }
}

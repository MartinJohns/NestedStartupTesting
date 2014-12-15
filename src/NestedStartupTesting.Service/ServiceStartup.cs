using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.DependencyInjection;

namespace NestedStartupTesting.Service
{
    public class ServiceStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IAssemblyProvider, ServiceAssemblyProvider>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}",
                    defaults: new { controller = "Home", action = "Index" });
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

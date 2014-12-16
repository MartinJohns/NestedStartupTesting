using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.DependencyInjection;
using NestedStartupTesting.Extensions;
using NestedStartupTesting.Service;

namespace NestedStartupTesting
{
    public class Startup
    {
        public ServiceStartup ServiceStartup { get; } = new ServiceStartup();

        public IServiceCollection ServiceCollectionCopy { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            ServiceCollectionCopy = services.CreateCopy();

            services.AddTransient<IAssemblyProvider, MainAssemblyProvider>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.IsolatedMap(
                new PathString("/api"),
                ServiceStartup.Configure,
                ServiceStartup.ConfigureServices,
                ServiceCollectionCopy);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}

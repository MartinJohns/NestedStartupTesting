using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Routing;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Runtime;
using NestedStartupTesting.Web.Extensions;
using NestedStartupTesting.Service;
using NestedStartupTesting.Shared;

namespace NestedStartupTesting.Web
{
    public class Startup
    {
        public Service.Startup ServiceStartup { get; } = new Service.Startup();

        public IServiceCollection ServiceCollectionCopy { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            ServiceCollectionCopy = new ServiceCollection { services };

            services.AddTransient(typeof(IAssemblyProvider), sp => new SingleAssemblyProvider(sp.GetService<ILibraryManager>(), typeof(Startup).Namespace));
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

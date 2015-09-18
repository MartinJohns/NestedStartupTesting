using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Dnx.Runtime;
using Microsoft.Framework.DependencyInjection;
using NestedStartupTesting.Shared;
using NestedStartupTesting.Web.Extensions;
using System;

namespace NestedStartupTesting.Web
{
    public class Startup
    {
        public Service.Startup ServiceStartup { get; } = new Service.Startup();

        public IServiceCollection ServiceCollectionCopy { get; set; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            ServiceCollectionCopy = new ServiceCollection();
            foreach (var service in services)
            {
                ServiceCollectionCopy.Add(service);
            }

            services.AddTransient(
                typeof(IAssemblyProvider),
                sp => new SingleAssemblyProvider(sp.GetService<ILibraryManager>(), typeof(Startup).Namespace));
            services.AddMvc();

            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.IsolatedMap<Service.Startup>(
                new PathString("/api"),
                ServiceCollectionCopy);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

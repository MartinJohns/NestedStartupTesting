using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.DependencyInjection;
using NestedStartupTesting.Web.Extensions;
using System;
using System.Reflection;

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
            
            services
                .AddMvc()
                .AddControllersAsServices(new[] { typeof(Startup).GetTypeInfo().Assembly });

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

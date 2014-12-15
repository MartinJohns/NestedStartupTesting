using System;
using System.Linq;
using Microsoft.Framework.DependencyInjection;

namespace NestedStartupTesting
{
    public class DummyServiceProvider : IServiceProvider
    {
        private readonly IServiceCollection _services;

        public DummyServiceProvider(IServiceCollection services)
        {
            _services = services;
        }

        public object GetService(Type serviceType)
        {
            var serviceDescriptor = _services.FirstOrDefault(x => x.ServiceType == serviceType);
            var instance = serviceDescriptor?.ImplementationFactory(this);
            return instance;
        }
    }
}
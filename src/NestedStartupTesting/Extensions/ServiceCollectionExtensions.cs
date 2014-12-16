using Microsoft.Framework.DependencyInjection;

namespace NestedStartupTesting.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection CreateCopy(this IServiceCollection serviceCollection)
        {
            var copy = new ServiceCollection();
            foreach (var service in serviceCollection)
            {
                copy.Add(service);
            }

            return copy;
        }
    }
}
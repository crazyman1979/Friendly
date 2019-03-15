using Microsoft.Extensions.DependencyInjection;

namespace Friendly.Core
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        ///     Gets a service implementation from the service collection
        /// </summary>
        /// <param name="services">Instance of IServiceCollection to search</param>
        /// <typeparam name="T">Type of service</typeparam>
        /// <returns>The implementation of T</returns>
        public static T GetService<T>(this IServiceCollection services)
        {
            return services.BuildServiceProvider().GetService<T>();
        }
    }
}
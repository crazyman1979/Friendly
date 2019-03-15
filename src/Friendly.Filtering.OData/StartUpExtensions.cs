using Friendly.Filtering.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Friendly.Filtering.OData
{
    public static class StartUpExtensions
    {
        public static IServiceCollection AddODataFiltering(this IServiceCollection services)
        {
            return services.AddScoped<IFilterParser, ODataFilterParser>();
        }
    }
}
using CodeFriendly.Filtering.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace CodeFriendly.Filtering.OData
{
    public static class StartUpExtensions
    {
        public static IServiceCollection AddFriendlyODataFiltering(this IServiceCollection services)
        {
            return services.AddScoped<IFilterParser, ODataFilterParser>();
        }
    }
}
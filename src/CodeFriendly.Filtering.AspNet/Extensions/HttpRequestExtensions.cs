using System.Linq;
using CodeFriendly.Filtering.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace CodeFriendly.Filtering.AspNet
{
    public static class HttpRequestExtensions
    {
        public static IFilterOptions GetFilterOptions(this HttpRequest request)
        {
            var parser = request.HttpContext.RequestServices.GetRequiredService<IFilterParser>();
            var options = parser.Parse(request.QueryString.Value?.TrimStart('?'));
            
            
            return new HttpFilterOptions(request.HttpContext.Response)
            {
                Expression = options.Expression,
                OffSet = options.OffSet,
                Limit = options.Limit,
                Sorts = options.Sorts
            };
        }

        
    }
}
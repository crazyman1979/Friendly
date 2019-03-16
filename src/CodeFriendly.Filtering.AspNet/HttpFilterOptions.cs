using System.Linq;
using CodeFriendly.Filtering.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace CodeFriendly.Filtering.AspNet
{
    public class HttpFilterOptions: FilterOptions, IFilterAppliedHandler
    {
        private readonly HttpResponse _response;

        public HttpFilterOptions(HttpResponse response)
        {
            _response = response;
        }
        public void Apply(FilteredResultSet resultSet)
        {
            if (resultSet != null)
            {
                var options = resultSet.Options;
                _response?.Headers?.Add(FilteringConstants.QUERY_TOTAL_ROW_COUNT_HEADER_NAME, new StringValues(resultSet.TotalCount.ToString()));
                _response?.Headers?.Add(FilteringConstants.QUERY_ROW_COUNT_HEADER_NAME, new StringValues(resultSet.LimitedCount.ToString()));
                _response?.Headers?.Add(FilteringConstants.QUERY_LIMIT_HEADER_NAME, new StringValues(options.Limit?.ToString()));
                _response?.Headers?.Add(FilteringConstants.QUERY_OFFSET_HEADER_NAME, new StringValues(options.OffSet?.ToString()));
                var sorts = (options?.Sorts ?? Enumerable.Empty<ISort>()).Select(s => s.ToString());
                _response?.Headers?.Add(FilteringConstants.QUERY_SORT_HEADER_NAME, new StringValues(string.Join(",", sorts )));
            }
        }
    }
}
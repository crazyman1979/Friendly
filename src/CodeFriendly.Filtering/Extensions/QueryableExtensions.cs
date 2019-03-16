using System.Linq;
using CodeFriendly.Filtering.Abstractions;
using System.Linq.Dynamic.Core;
// ReSharper disable SuspiciousTypeConversion.Global
namespace CodeFriendly.Filtering
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WithFilter<T>(this IQueryable<T> enumerable,
            IFilter<T> filter) where T : class
        {
            return enumerable.WithFilterResults(filter)?.Results;
        }
        public static FilteredResultSet<T> WithFilterResults<T>(this IQueryable<T> enumerable,
            IFilter<T> filter) where T: class
        {
            var items = (filter?.WhereExpression != null ? 
                enumerable.Where(filter.WhereExpression) : 
                enumerable).ToList();
            
            var filteredCount = items.Count;
            enumerable = items.AsQueryable();
            if (filter?.Options?.Sorts?.Any() ?? false)
            {
                var first = true;
                
                foreach (var sort in filter.Options.Sorts)
                {
                    enumerable = first
                        ? enumerable.OrderBy(sort.ToString())
                        : ((IOrderedQueryable<T>) enumerable).ThenBy(sort.ToString());
                    first = false;
                }
            }

            var take = filter?.Options?.Limit.GetValueOrDefault() ?? 0;
            var skip = filter?.Options?.OffSet.GetValueOrDefault() ?? 0;

            enumerable = skip > 0 ? enumerable.Skip(skip) : enumerable;
            enumerable = take > 0 ? enumerable.Take(take) : enumerable;

            var allResults = enumerable.ToList();
            
            var resultSet = new FilteredResultSet<T>(allResults.AsQueryable())
            {
                TotalCount = filteredCount,
                LimitedCount = allResults.Count,
                Options = filter?.Options
            };
            
            
            (filter as IFilterAppliedHandler ?? filter?.Options as IFilterAppliedHandler)?.Apply(resultSet);
            return resultSet;
        }
    }
}
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Friendly.Core;
using Friendly.Filtering.Abstractions;

namespace Friendly.Filtering
{
    public abstract class QueryStringFilterParserBase: IFilterParser
    {
        public IFilter<T> ParseAndBuild<T>(string expression) where T : class
        {
            var options = Parse(expression);
            return Build<T>(options);
        }
        public IFilter<T> Build<T>(IFilterOptions options) where T : class
        {
            return new Filter<T>
            {
                Options = options,
                WhereExpression = BuildWhereExpression<T>(options)
            };
        }

        public IFilterOptions Parse(string expression)
        {
            if (string.IsNullOrEmpty(expression)) return FilterOptions.Empty;
            
            var qs = HttpUtility.ParseQueryString(expression);
            var search = qs.Get("$filter");
            var sortArgs = qs.Get("sort");
            var limit = qs.Get("limit");
            var offset = qs.Get("offset");

            var sorts = sortArgs?.Split(",").Select(sa =>
            {
                var sortParts = sa.Split(" ");
                return new Sort
                {
                    PropertyName = sortParts.FirstOrDefault(),
                    Direction = sortParts.Skip(1).FirstOrDefault()
                };
            }) ?? Enumerable.Empty<Sort>();
            
            
            return new FilterOptions()
            {
                Limit = limit?.ToInt(),
                OffSet = offset?.ToInt(),
                Expression = search,
                Sorts = sorts
            };
        }

        

        protected abstract Expression<Func<T, bool>> BuildWhereExpression<T>(IFilterOptions options) where T : class;
    }
}
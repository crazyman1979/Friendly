using System;
using System.Linq.Expressions;
using Friendly.Filtering.Abstractions;

namespace Friendly.Filtering
{
    public class Filter<TModel>: IFilter<TModel> 
        where TModel: class
    {
        public IFilterOptions Options { get; set; }
        public Expression<Func<TModel, bool>> WhereExpression { get; set; }
    }
}
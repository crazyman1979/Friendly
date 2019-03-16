using System;
using System.Linq.Expressions;
using CodeFriendly.Filtering.Abstractions;

namespace CodeFriendly.Filtering
{
    public class Filter<TModel>: IFilter<TModel> 
        where TModel: class
    {
        public IFilterOptions Options { get; set; }
        public Expression<Func<TModel, bool>> WhereExpression { get; set; }
    }
}
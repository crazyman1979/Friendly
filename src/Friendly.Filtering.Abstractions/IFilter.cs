using System;
using System.Linq.Expressions;

namespace Friendly.Filtering.Abstractions
{
    public interface IFilter<TModel> 
        where TModel : class
    {
        IFilterOptions Options { get; }
        
        Expression<Func<TModel, bool>> WhereExpression { get; }
        
    }
}
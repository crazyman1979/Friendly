using System;
using System.Linq.Expressions;
using CodeFriendly.Filtering.Abstractions;

namespace CodeFriendly.Filtering.OData
{
    public class ODataFilterParser: QueryStringFilterParserBase
    {
        private readonly ExpressionBuilder _expressionBuilder = new ExpressionBuilder();
        protected override Expression<Func<T, bool>> BuildWhereExpression<T>(IFilterOptions options)
        {
            return !string.IsNullOrEmpty(options?.Expression) ?
                    _expressionBuilder.Build<T>(options.Expression) : null;
        }
    }
}
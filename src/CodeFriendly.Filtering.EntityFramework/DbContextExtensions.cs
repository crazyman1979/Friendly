using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using CodeFriendly.Filtering.Abstractions;

namespace CodeFriendly.Filtering.EntityFramework
{
    public static class DbContextExtensions
    {

        public static IQueryable<TEntity> WithFilter<TDomain, TEntity>(this IQueryable<TEntity> query,
            IFilter<TDomain> filter, IMapper mapper)
            where TEntity: class
            where TDomain: class
        {
            var serverQuery = filter?.WhereExpression?.ToDataFilter<TDomain, TEntity>(mapper);
            return serverQuery !=null ? query.Where(serverQuery) : query;
        }
        
        public static Expression<Func<TEntity, bool>> ToDataFilter<TDomain, TEntity>(this Expression<Func<TDomain, bool>> predicateExpression,
            IMapper mapper)
            where TEntity: class
            where TDomain: class
        {
            var domainFilter = Sprint.Filter.OData.Filter.Serialize(predicateExpression);
            var maps = mapper.ConfigurationProvider.GetAllTypeMaps();
            var map = maps.Single(m =>
                m.Types.SourceType == typeof(TDomain) && m.Types.DestinationType == typeof(TEntity));

            var conversions = map.MemberMaps.Select(m => new FilteringMap()
            {
                From = m.SourceMembers.Single().Name,
                To = m.DestinationName
            }).ToList();
            
            foreach (var c in conversions)
            {
                domainFilter = domainFilter.Replace(c.FromFormatted, c.ToFormatted, StringComparison.InvariantCultureIgnoreCase);
            }

            var formattedFilter = String.Empty;
            var items = domainFilter.Split(" and ")
                .Where(f => conversions.Any(c => f.IndexOf(c.To, StringComparison.InvariantCultureIgnoreCase) != -1))
                .ToList();

            formattedFilter = items.Any() ? items.Aggregate((p, c) => $"{p} and {c}") : formattedFilter;
            
            items = formattedFilter.Split(" or ")
                .Where(f => conversions.Any(c => f.IndexOf(c.To, StringComparison.InvariantCultureIgnoreCase) != -1))
                .ToList();

            formattedFilter = items.Any() ? items.Aggregate((p, c) => $"{p} or {c}") : formattedFilter;
            
            return Sprint.Filter.OData.Filter.Deserialize<TEntity>(formattedFilter);
            
        }
    }
}
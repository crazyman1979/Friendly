using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;

namespace Friendly.Core.AutoMapper
{
    public static class AutoMapperExtensions
    {
        public static ICollection<TTargetType> ResolveCollection<TSourceType, TTargetType>(this IMapper mapper,
            ICollection<TSourceType> sourceCollection,
            ICollection<TTargetType> targetCollection,
            Func<ICollection<TTargetType>, TSourceType, TTargetType> getMappingTargetFromTargetCollectionOrNull)
        {
            var existing = (targetCollection ?? Enumerable.Empty<TTargetType>()).ToList();
            targetCollection?.Clear();
            return ResolveCollection(mapper, sourceCollection, s => getMappingTargetFromTargetCollectionOrNull(existing, s), t => t);
        }

        private static ICollection<TTargetType> ResolveCollection<TSourceType, TTargetType>(
            IMapper mapper,
            IEnumerable<TSourceType> sourceCollection,
            Func<TSourceType, TTargetType> getMappingTargetFromTargetCollectionOrNull,
            Func<IList<TTargetType>, ICollection<TTargetType>> updateTargetCollection)
        {
            var updatedTargetObjects = 
                (sourceCollection ?? Enumerable.Empty<TSourceType>())
                .Select(sourceObject => new
                {
                    sourceObject, existingTargetObject = getMappingTargetFromTargetCollectionOrNull(sourceObject)
                })
                .Select(t =>
                    t.existingTargetObject == null
                        ? mapper.Map<TTargetType>(t.sourceObject)
                        : mapper.Map(t.sourceObject, t.existingTargetObject)).ToList();
            return updateTargetCollection(updatedTargetObjects);
        }

        public static IMappingExpression<TSource, TDestination> ForMemberCollection<TSource, TDestination, TSourceMember, TTargetMember>(
            this IMappingExpression<TSource, TDestination> expression,
            Expression<Func<TDestination, ICollection<TTargetMember>>> targetResolver,
            Expression<Func<TSource, ICollection<TSourceMember>>> sourceResolver,
            Func<ICollection<TTargetMember>, TSourceMember, TTargetMember> currentItemResolver )
        where TSource : class
        where TDestination: class
        
        {
            return expression.ForMember(targetResolver, o =>
            {
                o.MapFrom((source, target, member, ctx) =>
                {
                    var col =  ctx.Mapper.ResolveCollection(
                        sourceResolver.Compile().Invoke(source),
                        targetResolver.Compile().Invoke(target),
                        currentItemResolver);
                    return col;
                });
            });
        }
    }
}
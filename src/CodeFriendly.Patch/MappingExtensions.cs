using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;

namespace CodeFriendly.Patch
{
    public static class MappingExtensions
    {
        public static bool IsDirty(this IPatchStore store, object model, string propertyName)
        {
            return store.Get(model)?.Any(p => p.Equals(propertyName, StringComparison.OrdinalIgnoreCase)) ?? false;
        }


        public static void AddProfileFromPatchMapper<TPatchMapper>(this IMapperConfigurationExpression expression)
            where TPatchMapper: PatchMapper, new ()
        {
            expression.AddProfile(new TPatchMapper().Profile);
        }
        
        public static IMappingExpression<TSource, TDestination> ConfigureOwnedCollection<TSource, TDestination, TSourceMember, TTargetMember>(this IMappingExpression<TSource, TDestination> expression,
            Expression<Func<TDestination, ICollection<TTargetMember>>> targetResolver,
            Expression<Func<TSource, ICollection<TSourceMember>>> sourceResolver,
            Func<ICollection<TTargetMember>, TSourceMember, TTargetMember> currentItemResolver)
            where TSource : class 
            where TDestination : class
        {
            return expression.ForMember(targetResolver, o => { 
                o.MapFrom((source, target, member, ctx) =>
                {
                    var sourceCollection = sourceResolver.Compile().Invoke(source) ?? new List<TSourceMember>();
                    var targetCollection = targetResolver.Compile().Invoke(target) ?? new List<TTargetMember>();
                    var sourceToTargetMap = sourceCollection.Select(s => new { source = s, target = currentItemResolver(targetCollection, s)}).ToList();
                    var updates = sourceToTargetMap.Where(t => t.target != null).ToList();
                    var insert = sourceToTargetMap.Where(t => t.target == null).ToList();

                    foreach (var upd in updates)
                    {
                        
                        ctx.Mapper.Map(upd.source, upd.target, opts=>
                        {
                            var context = ctx.Options.Items.ContainsKey("context")
                                ? ctx.Options.Items["context"]
                                : null;
                            if (context != null)
                            {
                                opts.Items.Add("context", context);
                            }
                        });
                    }
                    foreach (var ins in insert)
                    {
                        var newItem = ctx.Mapper.Map<TTargetMember>(ins.source);
                        targetCollection.Add(newItem);
                    }
                    return targetCollection.ToList();
                    
                }); 
            });
        }
        
        
    }
}
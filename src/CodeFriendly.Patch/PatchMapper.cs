using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;

namespace CodeFriendly.Patch
{
    public abstract class PatchMapper
    {
        public Profile Profile { get; internal set; }
        public IMapper Mapper { get; internal set; }
        public IPatchStore Store { get; internal set; }
    }
    public abstract class PatchMapper<TSource, TDestination> : PatchMapper, IPatchMapper<TSource, TDestination> where TSource: class
        where TDestination: class
    {
        protected PatchMapper(): this(new PatchablePropertyStore()){}
        protected PatchMapper(IPatchStore store)
        {
            Store = store;
            Profile = new PatchProfile<TSource, TDestination>(this);
            Mapper = BuildMapper();
        }
        
        public TSource CreateSourceObject(TDestination entity)
        {
            var domain = Mapper.Map<TDestination,TSource>(entity);
            OnDestinationToSourceMapped(entity, domain);
            return domain;
        }

        public TDestination CreateDestinationObject(TSource domain)
        {
            var entity = Mapper.Map<TSource, TDestination>(domain);
            OnSourceToDestinationMapped(domain, entity);
            return entity;
        }

        public TDestination PatchDestinationObject<TPatch>(TPatch source, TDestination target)
            where TPatch: TSource
        {
            var entity = Mapper.Map(source, target, o=>o.Items.Add("context", new PatchMapContext(Store)));
            OnSourceToDestinationPatched(source, entity);
            return entity;
        }
        
        private IMapper BuildMapper()
        {
            var mapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(Profile);
                    OnConfigureProfiles(cfg);
                });
            return mapper.CreateMapper();
        }
        
        protected virtual void OnDestinationToSourceMapped(TDestination source, TSource target)
        {
        }
        
        protected virtual void OnSourceToDestinationMapped(TSource source, TDestination target)
        {
        }
        
        protected virtual void OnSourceToDestinationPatched(TSource source, TDestination target)
        {
        }

        protected internal virtual IMappingExpression<TSource, TDestination> OnConfigureMapper(IMappingExpression<TSource, TDestination> expression)
        {
            return expression;
        }
        
        // ReSharper disable once VirtualMemberNeverOverridden.Global
        protected internal virtual void OnConfigureReverseMapper(IMappingExpression<TDestination, TSource> expression)
        {
            
        }

        protected virtual void OnConfigureProfiles(IMapperConfigurationExpression expression)
        {
            
        }
    }
}
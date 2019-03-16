using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace CodeFriendly.Patch
{
    /// <inheritdoc />
    [SuppressMessage("ReSharper", "ClassWithVirtualMembersNeverInherited.Global")]
    public class PatchProfile<TDomain, TEntity> : Profile where TDomain: class
        where TEntity: class
    
    {

        public PatchProfile(PatchMapper<TDomain, TEntity> patchMapper)
        {
            var expression = CreateMap<TDomain, TEntity>();
            expression = patchMapper.OnConfigureMapper(expression);
            expression.ReverseMap();
            ForAllPropertyMaps(m => m.TypeMap.SourceType == typeof(TDomain) && m.TypeMap.DestinationType == typeof(TEntity), (pm, opt) =>
            {
                opt.Condition((source, destination, arg3, arg4, resolutionContext) => ShouldMap(source, resolutionContext, pm));
            });
            
        }

        protected virtual bool ShouldMap(object source, ResolutionContext resolutionContext, PropertyMap pm)
        {
            var context = resolutionContext.Options.Items.ContainsKey("context")
                ? resolutionContext.Options.Items["context"] as PatchMapContext
                : null;

            if (context != null)
            {
                return pm.SourceMembers.All(s => context.Store.IsDirty(source, s.Name));
            }

            return true;
        }
    }
}
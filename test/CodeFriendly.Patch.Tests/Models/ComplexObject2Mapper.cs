using System.Linq;
using AutoMapper;
using CodeFriendly.Patch.Tests.Models.Domain;
using CodeFriendly.Core.AutoMapper;
namespace CodeFriendly.Patch.Tests.Models
{
    public class ComplexObject2Mapper: PatchMapper<ComplexObject2, Models.Entity.ComplexObject2>
    {
        protected override IMappingExpression<ComplexObject2, Entity.ComplexObject2> 
            OnConfigureMapper(IMappingExpression<ComplexObject2, Entity.ComplexObject2> expression)
        {
            return expression.ConfigureOwnedCollection(t => t.Owned, s => s.Owned, (targetCollection, sourceObject) =>
            {
                return targetCollection.SingleOrDefault(t => t.Id == sourceObject.Id);
            });
        }

        protected override void OnConfigureProfiles(IMapperConfigurationExpression expression)
        {
            expression.AddProfileFromPatchMapper<SimpleObjectMapper>();
        }
    }
}
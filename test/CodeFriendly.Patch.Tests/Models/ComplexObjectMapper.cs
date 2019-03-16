using AutoMapper;

namespace CodeFriendly.Patch.Tests.Models
{
    public class ComplexObjectMapper: PatchMapper<Models.Domain.ComplexObject1, Models.Entity.ComplexObject1>
    {
        protected override void OnConfigureProfiles(IMapperConfigurationExpression expression)
        {
            expression.AddProfileFromPatchMapper<SimpleObjectMapper>();
        }
    }
}
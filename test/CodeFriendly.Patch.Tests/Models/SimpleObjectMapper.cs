using AutoMapper;

namespace CodeFriendly.Patch.Tests.Models
{
    public class SimpleObjectMapper: PatchMapper<Domain.SimpleObject, Entity.SimpleObject>
    {
        protected override void OnConfigureProfiles(IMapperConfigurationExpression expression)
        {
            expression.AddProfileFromPatchMapper<SimpleObjectMapper2>();
            base.OnConfigureProfiles(expression);
        }
    }
}
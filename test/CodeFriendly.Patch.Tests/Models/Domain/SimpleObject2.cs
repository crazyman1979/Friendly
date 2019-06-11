namespace CodeFriendly.Patch.Tests.Models.Domain
{
    public class SimpleObject2
    {
        public string Id { get; set; }

        public string MissingFromEntity { get; internal set; } = "XXX";
    }
}
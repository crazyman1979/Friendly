using System.Collections.Generic;

namespace Friendly.Patch.Tests.Models.Domain
{
    public class ComplexObject1: IPatchable
    {
        public string Property1 { get; set; }
        public SimpleObject Child { get; set; }
        public HashSet<string> DirtyProperties { get; set; }
        public string Id { get; set; }
    }
}
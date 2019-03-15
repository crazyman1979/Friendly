using System.Collections.Generic;

namespace Friendly.Patch.Tests.Models.Domain
{
    public class ComplexObject2: IPatchable
    {
        public List<SimpleObject> Owned { get; set; }
        
        public List<SimpleObject> References { get; set; }
        public HashSet<string> DirtyProperties { get; set; }
    }
}
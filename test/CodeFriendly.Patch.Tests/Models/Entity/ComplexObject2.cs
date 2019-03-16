using System.Collections.Generic;

namespace CodeFriendly.Patch.Tests.Models.Entity
{
    public class ComplexObject2
    {
        public List<SimpleObject> Owned { get; set; }

        public List<SimpleObject> References { get; set; }
    }
}
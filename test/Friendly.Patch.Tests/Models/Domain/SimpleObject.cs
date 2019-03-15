using System;
using System.Collections.Generic;

namespace Friendly.Patch.Tests.Models.Domain
{
    public class SimpleObject: IPatchable
    {
        public string Property1 { get; set; }
        public int Property2 { get; set; }
        public DateTime? Property3 { get; set; }
        public HashSet<string> DirtyProperties { get; set; }
        public string Id { get; set; }
    }
}
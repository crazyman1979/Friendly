using System;
using System.Collections.Generic;

namespace Friendly.Patch
{
    public interface IPatchable
    {
        HashSet<string> DirtyProperties { get; set; }
    }
}
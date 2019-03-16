using System;
using System.Collections.Generic;

namespace CodeFriendly.Patch
{
    public interface IPatchable
    {
        HashSet<string> DirtyProperties { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Friendly.Patch
{
    public class PatchablePropertyStore: IPatchStore
    {
        public void Set(object model, string property)
        {
            if (model is IPatchable p)
            {
                p.DirtyProperties = p.DirtyProperties ?? new HashSet<string>();
                p.DirtyProperties.Add(property);
            }
            else
            {
                throw new InvalidOperationException("PatchablePropertyStore required you to implement IPatchable");
            }
            
        }

        public void Set(object model, IEnumerable<string> properties)
        {
            if (model is IPatchable p)
            {
                p.DirtyProperties = p.DirtyProperties ?? new HashSet<string>();
                p.DirtyProperties = properties.ToHashSet();
            }
            else
            {
                throw new InvalidOperationException("PatchablePropertyStore required you to implement IPatchable");
            }
        }

        public IEnumerable<string> Get(object model)
        {
            if (model is IPatchable p)
            {
                return p.DirtyProperties;
            }
            throw new InvalidOperationException("PatchablePropertyStore required you to implement IPatchable");
        }
    }
}
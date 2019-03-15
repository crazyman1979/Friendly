

using System.Collections.Generic;

namespace Friendly.Patch
{
    public interface IPatchStore
    {
        void Set(object model, string property);
        void Set(object model, IEnumerable<string> properties);
        IEnumerable<string> Get(object model);
    }
}
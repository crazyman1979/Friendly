namespace Friendly.Patch
{
    public class PatchMapContext
    {
        public IPatchStore Store { get; set; }
        
        public PatchMapContext(IPatchStore store)
        {
            Store = store;
        }
        
    }
}
using System.Collections.Generic;
using CodeFriendly.Filtering.Abstractions;

namespace CodeFriendly.Filtering
{
    public class FilterOptions: IFilterOptions
    {
        public string Expression { get; set; }        
        public IEnumerable<ISort> Sorts { get; set; }
        public int? Limit { get; set; }
        public int? OffSet { get; set; }
        
        
        public static readonly FilterOptions Empty = new FilterOptions()
        {
            Sorts = new List<ISort>()
        };
    }
}
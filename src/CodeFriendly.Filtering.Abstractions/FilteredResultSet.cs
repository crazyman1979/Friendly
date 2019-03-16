using System.Linq;

namespace CodeFriendly.Filtering.Abstractions
{
    public class FilteredResultSet
    {
        public int TotalCount { get; set; }
        public int LimitedCount { get; set; }
        public IFilterOptions Options { get; set; }
    }
    
    
    public class FilteredResultSet<T> : FilteredResultSet
        where T: class
    {
        public IQueryable<T> Results { get; }

        public FilteredResultSet(IQueryable<T> results){
            Results = results;
        }
    }
}
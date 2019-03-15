namespace Friendly.Filtering.Abstractions
{
    public interface IFilterAppliedHandler
    {
        void Apply(FilteredResultSet resultSet);
    }
}
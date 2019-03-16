namespace CodeFriendly.Filtering.Abstractions
{
    public interface IFilterParser
    {
        IFilter<T> ParseAndBuild<T>(string expression) where T : class;
        IFilter<T> Build<T>(IFilterOptions options) where T : class;
        IFilterOptions Parse(string expression);
    }
}
namespace Friendly.Filtering.Abstractions
{
    public interface ISort
    {
        string PropertyName { get; set; }
        string Direction { get; set; }
    }
}
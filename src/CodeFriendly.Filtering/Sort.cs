using CodeFriendly.Filtering.Abstractions;

namespace CodeFriendly.Filtering
{
    public class Sort: ISort
    {
        public string PropertyName { get; set; }
        public string Direction { get; set; }

        public override string ToString()
        {
            return $"{PropertyName} {Direction ?? string.Empty}";
        }
    }
}
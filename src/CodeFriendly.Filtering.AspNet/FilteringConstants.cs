namespace CodeFriendly.Filtering.AspNet
{
    public static class FilteringConstants
    {
        public const string FILTER_PARAM_NAME = "$filter";
        public const string LIMIT_PARAM_NAME = "limit";
        public const string SORT_PARAM_NAME = "sort";
        public const string OFFSET_PARAM_NAME = "offset";
        
        public const string QUERY_TOTAL_ROW_COUNT_HEADER_NAME = "total-row-count";
        public const string QUERY_ROW_COUNT_HEADER_NAME = "row-count";
        public const string QUERY_OFFSET_HEADER_NAME = "filter-offset";
        public const string QUERY_LIMIT_HEADER_NAME = "filter-limit";
        public const string QUERY_SORT_HEADER_NAME = "filter-sort";
    }
}
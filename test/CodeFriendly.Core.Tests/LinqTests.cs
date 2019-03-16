using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CodeFriendly.Core.Tests
{
    public class LinqTests
    {
        [Fact]
        public void WhereNotNull_Filters_Nulls()
        {
            var sequence = new List<object>
            {
                "test",
                "test2",
                null,
                "test3",
                null
            };
            var result = sequence.WhereNotNull().ToList();
            Assert.Equal(3, result.Count);
        }
    }
}
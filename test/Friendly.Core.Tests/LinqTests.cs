using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Friendly.Core.Tests
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
            Assert.False(result.Any(s=> s == null));
        }
    }
}
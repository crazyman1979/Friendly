using System;
using Xunit;

namespace Friendly.Core.Tests
{
    public class ExceptionTests
    {
        [Fact]
        public void DumpMessage_Simple()
        {
            var ex = new Exception("INNER1");
            var message = ex.DumpMessage();
            Assert.Equal("INNER1", message);

        }
        [Fact]
        public void DumpMessage_Has_Inner_Exception_Details()
        {
            var ex = new Exception("INNER1");
            var ex2 = new Exception("INNER2", ex);
            var ex3 = new Exception("OUTER", ex2);

            var message = ex3.DumpMessage();
            Assert.Equal("INNER1 INNER2 OUTER", message);

        }
        
        [Fact]
        public void DumpMessage_IgnoreDuplicateInnerEx()
        {
            var ex = new Exception("INNER1");
            var ex2 = new Exception("INNER2", ex);
            var ex3 = new Exception("INNER2", ex2);
            var ex4 = new Exception("OUTER", ex3);

            var message = ex4.DumpMessage();
            Assert.Equal("INNER1 INNER2 OUTER", message);

        }
    }
}
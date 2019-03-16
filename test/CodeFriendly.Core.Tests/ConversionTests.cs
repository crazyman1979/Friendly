using System;
using Xunit;

namespace CodeFriendly.Core.Tests
{
    public class ConversionTests
    {
        [Fact]
        public void ToInt_Returns_Zero_For_Null_Input()
        {
            string test = null;
            var result = test.ToInt();
            Assert.Equal(0, result);
        }
        
        [Fact]
        public void ToInt_Returns_Zero_For_Empty_Input()
        {
            string test = "";
            var result = test.ToInt();
            Assert.Equal(0, result);
        }
        
        [Fact]
        public void ToInt_Returns_Correct_For_Int_Input()
        {
            string test = "234";
            var result = test.ToInt();
            Assert.Equal(234, result);
        }
        
        [Fact]
        public void ToInt_Returns_Correct_For_Int_Input_Negative()
        {
            string test = "-999";
            var result = test.ToInt();
            Assert.Equal(-999, result);
        }
        
        [Fact]
        public void ToInt_Throws_For_Decimal_Input()
        {
            string test = "4443.2334";
            Assert.Throws<FormatException>(() =>
            {
                var result = test.ToInt();
            });
        }
        
        [Fact]
        public void ToInt_Throws_For_Invalid_Input()
        {
            string test = "3243defsdf";
            Assert.Throws<FormatException>(() =>
            {
                var result = test.ToInt();
            });
        }
        
    }
}
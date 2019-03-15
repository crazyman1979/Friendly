using System;
using System.Collections.Generic;
using System.Linq;
using Friendly.Filtering.OData;
using Xunit;

namespace Friendly.Filtering.Tests
{
    public class ODataFilterParserTests
    {
        #region InitData
        private ODataFilterParser _filterParser = new ODataFilterParser();
        private List<TestModel1> testCollection1 = Enumerable.Range(0, 100).Select(r =>
            {
                return new TestModel1()
                {
                    Property_String = $"Testing{r}"
                };
            }).ToList();
        #endregion
        
        
        [Fact]
        public void ParseExpression_Equals_Single_Match()
        {
            var query = "$filter=Property_String eq 'Testing77'";
            var expression = _filterParser.ParseAndBuild<TestModel1>(query);
            var result = testCollection1.AsQueryable()
                .Where(expression.WhereExpression).ToList();
            Assert.Single(result);
        }
        
        [Fact]
        public void ParseExpression_Equals_No_Match()
        {
            var query = "$filter=Property_String eq 'Testing00000'";
            var expression = _filterParser.ParseAndBuild<TestModel1>(query);
            var result = testCollection1.AsQueryable()
                .Where(expression.WhereExpression).ToList();
            Assert.Empty(result);
        }
        
        [Fact]
        public void ParseExpression_Equals_Or_Match()
        {
            var query = "$filter=Property_String eq 'Testing1' or Property_String eq 'Testing2'";
            var expression = _filterParser.ParseAndBuild<TestModel1>(query);
            var result = testCollection1.AsQueryable()
                .Where(expression.WhereExpression).ToList();
            Assert.Equal(2, result.Count());
        }
    }
}
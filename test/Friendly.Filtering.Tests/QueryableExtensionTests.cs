using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Friendly.Filtering;
using Friendly.Filtering.Abstractions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Friendly.Filtering.AspNet;
using Friendly.Filtering.OData;
using Microsoft.Extensions.DependencyInjection;

namespace Friendly.Filtering.Tests
{
    public class QueryableExtensionTests
    {
        private List<TestModel1> testCollection1 = Enumerable.Range(0, 100).Select(r =>
        {
            return new TestModel1()
            {
                Property_String = $"Testing{r}",
                Property_Int = r
            };
        }).ToList();
        
        [Fact]
        public void WithFilter()
        {
            var filter = new Filter<TestModel1>()
            {
                Options = new FilterOptions()
                {
                    Limit = 10,
                    OffSet = 2,
                    Sorts = new []
                    {
                        new Sort()
                        {
                            Direction = "Desc",
                            PropertyName = "Property_Int"
                        }
                    }
                }, 
                WhereExpression = (TestModel1 model)=> model.Property_Int < 50
            };

            var results = testCollection1.AsQueryable()
                .WithFilter(filter).ToList();
            
            Assert.Equal(47, results.First().Property_Int);
            Assert.Equal(10, results.Count());
        }

        [Fact]
        public void BugFix_Offset1Only()
        {
            
            var filter = new Filter<TestModel1>()
            {
                Options = new FilterOptions()
                {
                    OffSet = 1
                }
            };

            var results = testCollection1.Take(1).AsQueryable()
                .WithFilter(filter).ToList();
            
            Assert.Empty(results);
        }

        [Fact]
        public void HttpFilterOptions_Apply()
        {
            var httpContextAccessor = Substitute.For<IHttpContextAccessor>();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IFilterParser, ODataFilterParser>();
            var request = httpContextAccessor.HttpContext.Request;
            var response = request.HttpContext.Response;
            request.HttpContext.RequestServices = serviceCollection.BuildServiceProvider();
            request.QueryString = QueryString.Create(new Dictionary<string, string>()
            {
                {FilteringConstants.LIMIT_PARAM_NAME, "1"},
                {FilteringConstants.OFFSET_PARAM_NAME, "1"}
            });
            var filterOptions = request.GetFilterOptions();
            
            Assert.NotNull(filterOptions);



            var parser = request.HttpContext.RequestServices.GetRequiredService<IFilterParser>();
            var filter = parser.Build<TestModel1>(filterOptions);

            response.Headers.Returns(new HeaderDictionary());
            var results = testCollection1.Take(2).AsQueryable()
                .WithFilter(filter).ToList();
            
            Assert.Single(results);
            
            
            
            Assert.Equal("1", response.Headers[FilteringConstants.QUERY_LIMIT_HEADER_NAME]);
            
            Assert.Equal("1", response.Headers[FilteringConstants.QUERY_OFFSET_HEADER_NAME]);
            
            Assert.Equal("1", response.Headers[FilteringConstants.QUERY_ROW_COUNT_HEADER_NAME]);
            
            Assert.Equal("2", response.Headers[FilteringConstants.QUERY_TOTAL_ROW_COUNT_HEADER_NAME]);
        }
    }
}
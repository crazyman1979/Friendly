using System;
using System.Collections.Generic;
using CodeFriendly.Patch.Tests.Models;
using CodeFriendly.Patch.Tests.Models.Entity;
using Xunit;

namespace CodeFriendly.Patch.Tests
{
    public class SimplePatchTests
    {
        
        

        [Fact]
        public void SimpleObject2_MapsEntityToDomain()
        {
            var x = new SimpleObject2();
            x.Id = "XXXX";
            
            
            var mapper = new SimpleObjectMapper2();

            var y = mapper.CreateSourceObject(x);
            
            Assert.Equal(y.Id, "XXXX");

        }
        
        
        [Fact]
        public void SimpleObject_MapsEntityToDomain()
        {
            var x = new SimpleObject()
            {
                SimpleObject2 = new SimpleObject2()
                {
                    Id = "XXXX"
                }
            };
            
            
            var mapper = new SimpleObjectMapper();

            var y = mapper.CreateSourceObject(x);
            
            Assert.Equal(y.SimpleObject2.Id, "XXXX");
        }
        
        
        [Fact]
        public void SimpleObject_MapsDomainToEntity_All()
        {
            var dte = DateTime.Now.Date;
            var obj = new Models.Domain.SimpleObject()
            {
                Property1 = "PROPERTY1",
                Property2 = 12345,
                Property3 = dte
            };

            var mapper = new SimpleObjectMapper();

            var entity = mapper.CreateDestinationObject(obj);
            
            Assert.Equal("PROPERTY1", entity.Property1);
            Assert.Equal(12345, entity.Property2);
            Assert.Equal(dte, entity.Property3);

        }
        
        
        
        [Fact]
        public void SimpleObject_PatchesDomainToEntity_All()
        {
            var dte = DateTime.Now.Date;
            var obj = new Models.Domain.SimpleObject()
            {
                Property1 = "PROPERTY1",
                Property2 = 12345,
                Property3 = dte,
                DirtyProperties = new HashSet<string>()
                {
                    "Property1",
                    "Property2",
                    "Property3"
                }
            };

            var mapper = new SimpleObjectMapper();

            var entity = new Models.Entity.SimpleObject()
            {
                Id = "1000",
                Property1 = "XXXXXX",
                Property2 = 99999,
                Property3 = null
            };

            var updatedEntity = mapper.PatchDestinationObject(obj, entity);
            
            Assert.Equal("1000", entity.Id);
            Assert.Equal("PROPERTY1", entity.Property1);
            Assert.Equal(12345, entity.Property2);
            Assert.Equal(dte, entity.Property3);

        }
        
        [Fact]
        public void SimpleObject_PatchesDomainToEntity_Partial()
        {
            var dte = DateTime.Now.Date;
            var dte2 = DateTime.Now.AddDays(-1);
            var obj = new Models.Domain.SimpleObject()
            {
                Property1 = "PROPERTY1",
                Property2 = 12345,
                Property3 = dte,
                DirtyProperties = new HashSet<string>()
                {
                    "Property1"
                }
            };

            var mapper = new SimpleObjectMapper();

            var entity = new Models.Entity.SimpleObject()
            {
                Id = "1000",
                Property1 = "XXXXXX",
                Property2 = 99999,
                Property3 = dte2
            };

            var updatedEntity = mapper.PatchDestinationObject(obj, entity);
            
            Assert.Equal("1000", entity.Id);
            Assert.Equal("PROPERTY1", entity.Property1);
            Assert.Equal(99999, entity.Property2);
            Assert.Equal(dte2, entity.Property3);

        }
    }
}
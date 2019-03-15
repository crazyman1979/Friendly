using System;
using System.Collections.Generic;
using System.Linq;
using Friendly.Patch.Tests.Models;
using Friendly.Patch.Tests.Models.Domain;
using Xunit;

namespace Friendly.Patch.Tests
{
    public class ComplexPatchTests2
    {
        [Fact]
        public void OwnedCollection_Simple_Update()
        {
            var domain = new Models.Domain.ComplexObject2()
            {
                Owned = new List<SimpleObject>()
                {
                    new SimpleObject()
                    {
                        Id = "123",
                        Property1 = "PROPERTY1",
                        Property2 = 12345,
                        Property3 = null,
                        DirtyProperties = new HashSet<string>()
                        {
                            "Property1",
                            "Property2",
                            "Property3"
                        }
                    }
                },
                DirtyProperties = new HashSet<string>()
                {
                    "Owned"
                }
            };
            
            var entity = new Models.Entity.ComplexObject2()
            {
                Owned = new List<Models.Entity.SimpleObject>()
                {
                    new Models.Entity.SimpleObject()
                    {
                        Id = "123",
                        Property1 = "OLD",
                        Property2 = 99999,
                        Property3 = DateTime.Now
                    }
                }
            };
            var mapper = new ComplexObject2Mapper();
            var updated = mapper.PatchDestinationObject(domain, entity);
            
            Assert.Equal("PROPERTY1", updated.Owned.Single().Property1);
            Assert.Equal(12345, updated.Owned.Single().Property2);
            Assert.Null(updated.Owned.Single().Property3);
        }
        
        [Fact]
        public void OwnedCollection_Simple_Update_Partial()
        {
            var domain = new Models.Domain.ComplexObject2()
            {
                Owned = new List<SimpleObject>()
                {
                    new SimpleObject()
                    {
                        Id = "123",
                        Property1 = "PROPERTY1",
                        Property2 = 12345,
                        Property3 = null,
                        DirtyProperties = new HashSet<string>()
                        {
                            "Property1"
                        }
                    }
                },
                DirtyProperties = new HashSet<string>()
                {
                    "Owned"
                }
            };
            var dt = DateTime.Now;
            var entity = new Models.Entity.ComplexObject2()
            {
                Owned = new List<Models.Entity.SimpleObject>()
                {
                    new Models.Entity.SimpleObject()
                    {
                        Id = "123",
                        Property1 = "OLD",
                        Property2 = 99999,
                        Property3 = dt
                    }
                }
            };
            var mapper = new ComplexObject2Mapper();
            var updated = mapper.PatchDestinationObject(domain, entity);
            
            Assert.Equal("PROPERTY1", updated.Owned.Single().Property1);
            Assert.Equal(99999, updated.Owned.Single().Property2);
            Assert.Equal(dt, updated.Owned.Single().Property3);
        }
        
        [Fact]
        public void OwnedCollection_NewItem()
        {
            var domain = new Models.Domain.ComplexObject2()
            {
                Owned = new List<SimpleObject>()
                {
                    new SimpleObject()
                    {
                        Id = "123",
                        Property1 = "PROPERTY1",
                        Property2 = 12345,
                        Property3 = null,
                        DirtyProperties = new HashSet<string>()
                        {
                            "Property1"
                        }
                    }
                },
                DirtyProperties = new HashSet<string>()
                {
                    "Owned"
                }
            };
            var dt = DateTime.Now;
            var entity = new Models.Entity.ComplexObject2();
            var mapper = new ComplexObject2Mapper();
            var updated = mapper.PatchDestinationObject(domain, entity);
            
            Assert.Equal("PROPERTY1", updated.Owned.Single().Property1);
            Assert.Equal(12345, updated.Owned.Single().Property2);
            Assert.Null(updated.Owned.Single().Property3);
        }
    }
}
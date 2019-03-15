using System;
using System.Collections.Generic;
using Friendly.Patch.Tests.Models;
using Friendly.Patch.Tests.Models.Domain;
using Xunit;

namespace Friendly.Patch.Tests
{
    public class ComplexPatchTests1
    {
        [Fact]
        public void ComplexObject1_MapsDomainToEntity_All()
        {
            var dte = DateTime.Now.Date;
            var obj = new Models.Domain.ComplexObject1()
            {
                Property1 = "PROPERTY1",
                Child = new SimpleObject()
                {
                    Property1 = "PROPERTY11",
                    Property2 = 12345,
                    Property3 = dte
                }
            };

            var mapper = new ComplexObjectMapper();

            var entity = mapper.CreateDestinationObject(obj);
            
            Assert.Equal("PROPERTY1", entity.Property1);
            Assert.Equal("PROPERTY11", entity.Child.Property1);
            Assert.Equal(12345, entity.Child.Property2);
            Assert.Equal(dte, entity.Child.Property3);

        }

        [Fact]
        public void ComplexObject1_PatchDomainToEntity_All()
        {
            var dte = DateTime.Now.Date;
            var obj = new Models.Domain.ComplexObject1()
            {
                Property1 = "PROPERTY1",
                Child = new SimpleObject()
                {
                    Property1 = "PROPERTY11",
                    Property2 = 12345,
                    Property3 = dte,
                    DirtyProperties = new HashSet<string>()
                    {
                        "PRoperty1",
                        "PRoperty2",
                        "PRoperty3"
                    }
                },
                DirtyProperties = new HashSet<string>()
                {
                    "PRoperty1",
                    "Child"
                }
            };

            var mapper = new ComplexObjectMapper();

            var entity = new Models.Entity.ComplexObject1()
            {
                Property1 = "OLD",
                Child = new Models.Entity.SimpleObject()
                {
                    Property1 = "OLD2",
                    Property2 = 7647567,
                    Property3 = null
                }
            };

            var updatedEntity = mapper.PatchDestinationObject(obj, entity);
            
            Assert.Equal("PROPERTY1", entity.Property1);
            Assert.Equal(12345, entity.Child.Property2);
            Assert.Equal("PROPERTY11", entity.Child.Property1);
            Assert.Equal(dte, entity.Child.Property3);
            

        }
        
        
        [Fact]
        public void ComplexObject1_PatchDomainToEntity_Partial()
        {
            var dte = DateTime.Now.Date;
            var obj = new Models.Domain.ComplexObject1()
            {
                Property1 = "PROPERTY1",
                Child = new SimpleObject()
                {
                    Property1 = "PROPERTY11",
                    Property2 = 12345,
                    Property3 = dte,
                    DirtyProperties = new HashSet<string>()
                    {
                        "PRoperty2",
                    }
                },
                DirtyProperties = new HashSet<string>()
                {
                    "Child"
                }
            };

            var mapper = new ComplexObjectMapper();

            var entity = new Models.Entity.ComplexObject1()
            {
                Property1 = "OLD",
                Child = new Models.Entity.SimpleObject()
                {
                    Property1 = "OLD2",
                    Property2 = 7647567,
                    Property3 = null
                }
            };

            var updatedEntity = mapper.PatchDestinationObject(obj, entity);
            
            Assert.Equal("OLD", entity.Property1);
            Assert.Equal(12345, entity.Child.Property2);
            Assert.Equal("OLD2", entity.Child.Property1);
            Assert.Null(entity.Child.Property3);
            

        }
        
        [Fact]
        public void ComplexObject1_PatchDomainToEntity_Partial_IgnoreFullChild()
        {
            var dte = DateTime.Now.Date;
            var obj = new Models.Domain.ComplexObject1()
            {
                Property1 = "PROPERTY1",
                Child = new SimpleObject()
                {
                    Property1 = "PROPERTY11",
                    Property2 = 12345,
                    Property3 = dte,
                    DirtyProperties = new HashSet<string>()
                    {
                        "PRoperty2",
                    }
                },
                DirtyProperties = new HashSet<string>()
                {
                    "Property1",
                    "Child"
                }
            };

            var mapper = new ComplexObjectMapper();

            var entity = new Models.Entity.ComplexObject1()
            {
                Property1 = "OLD",
                Child = new Models.Entity.SimpleObject()
                {
                    Property1 = "OLD2",
                    Property2 = 7647567,
                    Property3 = null
                }
            };

            var updatedEntity = mapper.PatchDestinationObject(obj, entity);
            
            Assert.Equal("PROPERTY1", entity.Property1);
            Assert.Equal(12345, entity.Child.Property2);
            Assert.Equal("OLD2", entity.Child.Property1);
            Assert.Null(entity.Child.Property3);
            

        }
        
        
    }
}
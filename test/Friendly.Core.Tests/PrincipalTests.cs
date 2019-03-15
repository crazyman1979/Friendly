using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace Friendly.Core.Tests
{
    public class PrincipalTests
    {
        [Fact]
        public void HasScope_Returns_True_When_Has_Single_Scope()
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
            {
                new Claim("scope", "single-scope")
            }));
            var hasScope = principal.HasScope("single-scope");
            Assert.True(hasScope);
        }
        
        [Fact]
        public void HasScope_Returns_True_When_Has_Multiple_Scope()
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
            {
                new Claim("scope", "scope1 scope2 scope3")
            }));
            var hasScope = principal.HasScope("scope2");
            Assert.True(hasScope);
        }
        
        [Fact]
        public void HasScope_Returns_False_When_Not_Has_Single_Scope()
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
            {
                new Claim("scope", "single-scope")
            }));
            var hasScope = principal.HasScope("single-scopeXX");
            Assert.False(hasScope);
        }
        
        [Fact]
        public void HasScope_Returns_False_When_Not_Has_No_Scope()
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()));
            var hasScope = principal.HasScope("single-scopeXX");
            Assert.False(hasScope);
        }
    }
}
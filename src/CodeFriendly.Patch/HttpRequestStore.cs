using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace CodeFriendly.Patch
{
    public class HttpRequestStore: IPatchStore
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpRequestStore(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public void Set(object model, string property)
        {
            BuildOrGetHashset(model).Add(property);
        }

        public void Set(object model, IEnumerable<string> properties)
        {
            foreach (var prop in properties)
            {
                Set(model, prop);
            }
        }

        public IEnumerable<string> Get(object model)
        {
            return BuildOrGetHashset(model);
        }

        private HashSet<string> BuildOrGetHashset(object model)
        {
            
            if (!_contextAccessor.HttpContext.Items.ContainsKey("Friendly_Patch"))
            {
                _contextAccessor.HttpContext.Items.Add("Friendly_Patch", new Dictionary<object,HashSet<string>>());
            }
            var dict = _contextAccessor.HttpContext.Items["Friendly_Patch"] as Dictionary<object, HashSet<string>>;
            if (!dict.ContainsKey(model))
            {
                dict.Add(model, new HashSet<string>());
            }

            return dict[model];
        }
    }
}
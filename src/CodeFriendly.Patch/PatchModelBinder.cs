using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CodeFriendly.Core;
using CodeFriendly.Patch;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;

namespace CodeFriendly.Patch
{
    public class PatchModelBinder: IModelBinder
    {
        private readonly IModelBinder _innerBinder;
        private readonly IPatchStore _patchStore;

        public PatchModelBinder(IModelBinder innerBinder, IPatchStore patchStore)
        {
            _innerBinder = innerBinder;
            _patchStore = patchStore;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            bindingContext.HttpContext.Request.EnableRewind();
            using (var reader = new StreamReader(bindingContext.HttpContext.Request.Body))
            {
                var body = reader.ReadToEnd();

                var jobj = JObject.Parse(body);
 
                bindingContext.HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);
                await _innerBinder.BindModelAsync(bindingContext);
                if (bindingContext.Result.Model != null)
                {
                    ApplyPatchedProps(bindingContext.Result.Model, jobj);
                }
                
            } 
        }

        private void ApplyPatchedProps(object model, JObject original)
        {
            foreach (var p in original.Properties())
            {
                if (p.Value is JObject inner)
                {
                    var innerModel = model.GetPropertyValue(p.Path, true);
                    if (innerModel !=null)
                    {
                        ApplyPatchedProps(innerModel, inner);
                    }
                }
                _patchStore.Set(model, p.Name);
                
            }
        }
    }
}
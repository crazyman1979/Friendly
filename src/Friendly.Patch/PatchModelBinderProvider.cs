using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Friendly.Patch
{
    public class PatchModelBinderProvider: IModelBinderProvider
    {
        private readonly IList<IModelBinderProvider> _providers;
        private readonly IPatchStore _patchStore;

        public PatchModelBinderProvider(IList<IModelBinderProvider> providers, IPatchStore patchStore)
        {
            _providers = providers;
            _patchStore = patchStore;
        }
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (!context.Metadata.ModelType.IsClass ||
                !typeof(IPatchable).IsAssignableFrom(context.Metadata.ModelType)) return null;
            var bodyBinderProvider = _providers.OfType<BodyModelBinderProvider>().Single();
            var bodyBinder = bodyBinderProvider.GetBinder(context);
            return new PatchModelBinder(bodyBinder, _patchStore);

        }
    }
}
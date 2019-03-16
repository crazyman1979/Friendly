using CodeFriendly.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;


namespace CodeFriendly.Patch
{
    public static class StartupExtensions
    {
        public static MvcOptions AddPatchModelBinder(this MvcOptions options, IPatchStore patchStore)
        {
            options.ModelBinderProviders.Insert(0, new PatchModelBinderProvider(options.ModelBinderProviders, patchStore));
            return options;
        }

        public static IMvcCoreBuilder AddFriendlyPatchSupport(this IMvcCoreBuilder builder)
        {
            builder.Services.AddScoped<IPatchStore, HttpRequestStore>();
            return builder.AddMvcOptions(opts => opts.AddPatchModelBinder(builder.Services.GetService<IPatchStore>()));
        }
    }
}
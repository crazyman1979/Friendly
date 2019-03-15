using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Friendly.Filtering.Abstractions;
using Friendly.Patch;

namespace Friendly.ServiceModel.Abstractions
{
    public interface IService<TDomain> 
    {
        Task<TDomain> Get(params object [] keyValues);
        Task<IEnumerable<TDomain>> GetAll(IFilterOptions filterOptions);
        Task<TDomain> Create(TDomain createModel);
        Task<TDomain> Patch<T>(T updateModel) where T: TDomain, IPatchable;
    }
}
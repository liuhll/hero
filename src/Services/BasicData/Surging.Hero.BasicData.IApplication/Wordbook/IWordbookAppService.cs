using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Core.System.Intercept;
using Surging.Hero.BasicData.IApplication.Wordbook.Dtos;
using Surging.Hero.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Surging.Hero.BasicData.IApplication.Wordbook
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface IWordbookAppService : IServiceKey
    {
        [HttpPost(true)]
        [ServiceRoute("create")]
        Task<string> Create(CreateWordbookInput input);

        [HttpPut(true)]
        [ServiceRoute("update")]
        [InterceptMethod(CachingMethod.Remove, CorrespondingKeys = new string[] { CacheKeyConstant.GetWordBookById }, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<string> Update(UpdateWordbookInput input);

        [ServiceRoute("delete/{id}")]
        [HttpDelete(true)]
        [InterceptMethod(CachingMethod.Remove, CorrespondingKeys = new string[] { CacheKeyConstant.GetWordBookById }, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<string> Delete([CacheKey(1)]long id);


        [ServiceRoute("query")]
        Task<IPagedResult<GetWordbookOutput>> Query(QueryWordbookInput query);

        [ServiceRoute("get/{id}")]
        [HttpGet(true)]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetWordBookById, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<GetWordbookOutput> Get([CacheKey(1)]long id);

        [ServiceRoute("items/id/{wordbookId}")]
        [HttpGet(true)]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetWordBookItemsById, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<IEnumerable<GetWordbookItemOutput>> GetWordbookItems([CacheKey(1)]long wordbookId);

        [HttpPost(true)]
        [ServiceRoute("items/create")]
        Task<string> CreateWordbookItem(CreateWordbookItemInput input);

        [HttpPut(true)]
        [ServiceRoute("items/update")]
        [InterceptMethod(CachingMethod.Remove, CorrespondingKeys = new string[] { CacheKeyConstant.GetWordBookItemsById,CacheKeyConstant.GetWordBookItemById, CacheKeyConstant.RemoveGetWordBookItem, CacheKeyConstant.RemoveGetWordBookItems }, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<string> UpdateWordbookItem(UpdateWordbookItemInput input);

        [ServiceRoute("items/delete/{id}")]
        [HttpDelete(true)]
        [InterceptMethod(CachingMethod.Remove, CorrespondingKeys = new string[] { CacheKeyConstant.GetWordBookItemsById, CacheKeyConstant.GetWordBookItemById, CacheKeyConstant.RemoveGetWordBookItem,CacheKeyConstant.RemoveGetWordBookItems }, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<string> DeleteWordbookItem([CacheKey(1)]long id);

        [ServiceRoute("items/get/{id}")]
        [HttpGet(true)]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetWordBookItemById, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<GetWordbookItemOutput> GetWordbookItem([CacheKey(1)]long id);

        [HttpPost(true)]
        [ServiceRoute("check")]
        Task<bool> Check(CheckWordbookInput input);

        [ServiceRoute("items/code/{code}")]
        [HttpGet(true)]
        [InterceptMethod(CachingMethod.Get, Key = CacheKeyConstant.GetWordBookItemsByCode, Mode = Core.Caching.CacheTargetType.Redis)]
        Task<IEnumerable<GetWordbookItemOutput>> GetWordbookItemByCode([CacheKey(1)]string code);
    }
}

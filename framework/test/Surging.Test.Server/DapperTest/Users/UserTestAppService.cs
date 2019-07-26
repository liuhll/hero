using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.Dapper.Repositories;
using Surging.Core.Domain.PagedAndSorted;
using Surging.Core.Domain.PagedAndSorted.Extensions;
using Surging.Core.ProxyGenerator;

namespace Surging.Test.Server.DapperTest.Users
{
    public class UserTestAppService : ProxyServiceBase, IUserTestAppService
    {
        private readonly IDapperRepository<UserInfo, long> _userRepository;
        public UserTestAppService(IDapperRepository<UserInfo, long> userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IPagedResult<UserInfo>> Query(QueryUserInfoInput query)
        {
            //var queryResult = await _userRepository.GetAllAsync(p => p.UserName.Contains(query.SearchKey));
            //return queryResult.PageBy(query);
            var queryResult1 = await _userRepository.GetPageAsync(p => p.UserName.Contains(query.SearchKey), query.PageIndex, query.PageCount);
            return queryResult1.Item1.GetPagedResult(queryResult1.Item2);
        }
    }
}

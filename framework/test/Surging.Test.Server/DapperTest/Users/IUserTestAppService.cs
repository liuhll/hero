using Surging.Core.CPlatform.Ioc;
using Surging.Core.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Core.Domain.PagedAndSorted;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Surging.Test.Server.DapperTest.Users
{
    [ServiceBundle("v1/api/usertest")]
    public interface IUserTestAppService : IServiceKey
    {
        Task<IPagedResult<UserInfo>> Query(QueryUserInfoInput query);
    }
}

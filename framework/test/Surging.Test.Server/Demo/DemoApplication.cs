using Surging.Core.CPlatform.Exceptions;
using Surging.Core.CPlatform.Ioc;
using Surging.Core.ProxyGenerator;
using Surging.Test.Server.Demo.Dtos;
using System;
using System.Threading.Tasks;

namespace Surging.Test.Server.Demo
{
    [ModuleName("demo.v1",Version = "v1")]
    public class DemoApplication : ProxyServiceBase, IDemoApplication
    {
        public async Task<GetUserInfoOutput> GetUserInfo(string userId)
        {
            return new GetUserInfoOutput() {
                Id = userId,
                UserName = "zhangsan",
                ChineseName = "张三"
            };
        }

        public async Task<string> TestError()
        {
            throw new UserFriendlyException("测试抛出异常");
        }

        public async Task<string> TestServiceRoute(long workbooId)
        {
            return workbooId.ToString();
        }
    }
}

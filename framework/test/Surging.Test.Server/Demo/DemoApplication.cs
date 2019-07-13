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
    }
}

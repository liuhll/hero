using Surging.Core.CPlatform.Runtime.Session;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.ProxyGenerator;
using System.Threading.Tasks;
using System.Collections.Generic;
using Surging.Core.CPlatform.Runtime;

namespace Surging.Hero.Common.Runtime.Session
{
    public static class ISurgingSessionExtension
    {
        private const string getLoginUserInfoApi = "api/account/userinfo";
        private const string accountServiceKey = "account.v1";

        public static async Task<LoginUserInfo> GetLoginUserInfo(this ISurgingSession session) {

            if (session == null || !session.UserId.HasValue) {
                throw new BusinessException("您没有登录系统或登录超时,请先登录系统");
            }
            var serviceProxyProvider = ServiceLocator.GetService<IServiceProxyProvider>();
            var loginUser = await serviceProxyProvider.Invoke<LoginUserInfo>(new Dictionary<string, object>() { },getLoginUserInfoApi,HttpMethod.GET, accountServiceKey);
            return loginUser;
        }
    }
}

using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Exceptions;
using Surging.Core.CPlatform.Runtime;
using Surging.Core.CPlatform.Runtime.Session;
using Surging.Core.CPlatform.Transport.Implementation;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.ProxyGenerator;
using Surging.Hero.Auth.Domain.Shared;

namespace Surging.Hero.Common.Runtime.Session
{
    public static class ISurgingSessionExtension
    {
        private const string getLoginUserInfoApi = "api/account/userinfo";
        private const string accountServiceKey = "account.v1";

        public static async Task<LoginUserInfo> GetLoginUserInfo(this ISurgingSession session)
        {
            if (session == null || !session.UserId.HasValue) throw new BusinessException("您没有登录系统或登录超时,请先登录系统");
            var serviceProxyProvider = ServiceLocator.GetService<IServiceProxyProvider>();
            var loginUser = await serviceProxyProvider.Invoke<LoginUserInfo>(new Dictionary<string, object>(),
                getLoginUserInfoApi, HttpMethod.GET, accountServiceKey);
            return loginUser;
        }

        public static DataPermissionType GetLoginUserDataPermission(this ISurgingSession session)
        {
            if (session == null || !session.UserId.HasValue) throw new BusinessException("您没有登录系统或登录超时,请先登录系统");
            var dataPermissionTypeDesc = RpcContext.GetContext().GetAttachment(ClaimTypes.DataPermission);
            if (dataPermissionTypeDesc == null || (string) dataPermissionTypeDesc == "")
            {
                throw new BusinessException("获取当前登录用户的数据权限失败");
            }
            return Enum.Parse<DataPermissionType>(dataPermissionTypeDesc.ToString());
        }

        public static void CheckLoginUserDataPermision(this ISurgingSession session)
        {
            session.CheckLoginUserDataPermision("您没有插入数据的权限");
        }
        
        public static void CheckLoginUserDataPermision(this ISurgingSession session,long? orgId, string message)
        {
            if (session == null || !session.UserId.HasValue) throw new BusinessException("您没有登录系统或登录超时,请先登录系统");
            if (!session.IsAllOrg && (session.DataPermissionOrgIds == null 
                                      || !orgId.HasValue
                                      || !session.OrgId.HasValue 
                                      || !session.DataPermissionOrgIds.Contains(orgId.Value)))
            {
                throw new BusinessException(message);
            }
        }
        
        public static void CheckLoginUserDataPermision(this ISurgingSession session,string message)
        {
            session.CheckLoginUserDataPermision(session.OrgId,message);
        }

        public static void CheckLoginUserDataPermision(this ISurgingSession session, DataPermissionType? dataPermissionType,string message)
        {
            if (dataPermissionType.HasValue && dataPermissionType > session.GetLoginUserDataPermission())
            {
                throw new BusinessException(message);
            }
        }
        
        public static void CheckLoginUserDataPermision(this ISurgingSession session, DataPermissionType? dataPermissionType)
        {
            if (dataPermissionType.HasValue && dataPermissionType > session.GetLoginUserDataPermission())
            {
                throw new BusinessException("您要设置的数据权限大于您拥有的数据权限,系统不允许该操作");
            }
        }
    }
}
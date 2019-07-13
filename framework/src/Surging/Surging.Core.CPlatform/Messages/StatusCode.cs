using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Core.CPlatform.Messages
{
    public enum StatusCode
    { /// <summary>
      /// 成功调用
      /// </summary>
        OK = 200,

        /// <summary>
        /// 通信错误
        /// </summary>
        CommunicationError = 501,

        /// <summary>
        /// 平台架构异常
        /// </summary>
        CPlatformError = 502,

        /// <summary>
        /// 业务处理异常
        /// </summary>
        BusinessError = 503,

        /// <summary>
        /// 输入错误
        /// </summary>
        ValidateError = 504,

        /// <summary>
        /// 数据访问错误
        /// </summary>
        DataAccessError = 505,

        /// <summary>
        /// 用户友好类异常
        /// </summary>
        UserFriendly = 506,

        /// <summary>
        /// 未被认证
        /// </summary>
        UnAuthentication = 401,

        /// <summary>
        /// 未授权
        /// </summary>
        UnAuthorized = 402,

        /// <summary>
        /// 请求错误
        /// </summary>
        RequestError = 403,

        /// <summary>
        /// 未知错误
        /// </summary>
        UnKnownError = -1,
    }
}

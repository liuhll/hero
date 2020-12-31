using System.Threading.Tasks;
using Nest;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Cloud.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Hero.Common;

namespace Surging.Hero.BasicData.IApplication.Captcha
{
    [ServiceBundle(HeroConstants.RouteTemplet)]
    public interface ICaptchaAppService : IServiceKey
    {
        /// <summary>
        ///  获取验证码参数
        /// </summary>
        /// <param name="uuid">表单唯一id</param>
        /// <returns></returns>
        [HttpGet]
        [ServiceRoute("")]
        [Service(EnableAuthorization = false, Director = Developers.Liuhll,Name = "获取随机验证码", Date = "2020-12-31")]
        Task<string> GetCaptcha(string uuid);
    }
}
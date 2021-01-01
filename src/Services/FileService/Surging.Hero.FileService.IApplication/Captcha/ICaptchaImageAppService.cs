using System.Threading.Tasks;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Cloud.CPlatform.Runtime.Server.Implementation.ServiceDiscovery.Attributes;
using Surging.Cloud.KestrelHttpServer;
using Surging.Hero.Common;

namespace Surging.Hero.FileService.IApplication.Captcha
{
    [ServiceBundle(HeroConstants.CaptachaRouteTemplet)]
    public interface ICaptchaImageAppService : IServiceKey
    {
        [HttpGet]
        [ServiceRoute("{uuid}")]
        [Service(EnableAuthorization = false, Director = Developers.Liuhll,Name = "获取随机验证码", Date = "2021-1-1")]
        Task<IActionResult> GetCaptcha(string uuid);
    }
}
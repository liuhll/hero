using System.Runtime.Serialization;
using System.Threading.Tasks;
using Surging.Cloud.KestrelHttpServer;
using Surging.Cloud.ProxyGenerator;
using Surging.Hero.FileService.Domain.Captcha;
using Surging.Hero.FileService.IApplication.Captcha;

namespace Surging.Hero.FileService.Application.Captcha
{
    public class CaptchaAppService : ProxyServiceBase, ICaptchaAppService
    {
        private readonly ICaptchaDomainService _captchaDomainService;

        public CaptchaAppService(ICaptchaDomainService captchaDomainService)
        {
            _captchaDomainService = captchaDomainService;
        }

        public async Task<IActionResult> GetCaptcha(string uuid)
        {
            return await _captchaDomainService.GetCaptcha(uuid);
        }
    }
}
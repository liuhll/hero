using System.Runtime.Serialization;
using System.Threading.Tasks;
using Surging.Cloud.KestrelHttpServer;
using Surging.Cloud.ProxyGenerator;
using Surging.Hero.FileService.Domain.Captchas;
using Surging.Hero.FileService.IApplication.Captcha;

namespace Surging.Hero.FileService.Application.Captcha
{
    public class CaptchaImageAppService : ProxyServiceBase, ICaptchaImageAppService
    {
        private readonly ICaptchaDomainService _captchaDomainService;
        
        public CaptchaImageAppService(ICaptchaDomainService captchaDomainService)
        {
            _captchaDomainService = captchaDomainService;
        }

        public async Task<IActionResult> GetCaptcha(string uuid)
        {
            return await _captchaDomainService.GetCaptcha(uuid);
        }
    }
}
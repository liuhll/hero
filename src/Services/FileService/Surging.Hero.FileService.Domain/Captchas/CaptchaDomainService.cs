using System.Threading.Tasks;
using Surging.Cloud.Dapper.Manager;
using Surging.Cloud.KestrelHttpServer;
using Surging.Hero.BasicData.IApplication.Captcha;

namespace Surging.Hero.FileService.Domain.Captchas
{
    public class CaptchaDomainService : ManagerBase, ICaptchaDomainService
    {
        public async Task<IActionResult> GetCaptcha(string uuid)
        {
            var captchaAppServiceProxy = GetService<ICaptchaAppService>();
            var captchaCode = await captchaAppServiceProxy.GetCaptcha(uuid);
            var captchaBytes = Utils.CreateCaptcha(captchaCode);
            return new ImageResult(captchaBytes,"image/png");
                
        }
    }
}
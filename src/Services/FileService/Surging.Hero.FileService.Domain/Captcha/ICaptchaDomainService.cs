using System.Threading.Tasks;
using Surging.Cloud.CPlatform.Ioc;
using Surging.Cloud.KestrelHttpServer;

namespace Surging.Hero.FileService.Domain.Captcha
{
    public interface ICaptchaDomainService : ITransientDependency
    {
        Task<IActionResult> GetCaptcha(string uuid);
    }
}
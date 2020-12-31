using System.Threading.Tasks;
using Surging.Cloud.Caching;
using Surging.Cloud.CPlatform.Cache;
using Surging.Cloud.ProxyGenerator;
using Surging.Hero.BasicData.IApplication.Captcha;
using Surging.Hero.Common;
using Surging.Hero.Common.Utils;

namespace Surging.Hero.BasicData.Application.Captcha
{
    public class CaptchaAppService : ProxyServiceBase, ICaptchaAppService
    {
        private readonly ICacheProvider _cacheProvider;
        private const int CaptchaLength = 5;

        public CaptchaAppService()
        {
            _cacheProvider = CacheContainer.GetService<ICacheProvider>(HeroConstants.CacheProviderKey);;
        }

        public async Task<string> GetCaptcha(string uuid)
        {
            var captchaCode = IdentifyCodeGenerator.Create(CaptchaLength, IdentifyCodeType.MixNumberLetter);
            await _cacheProvider.AddAsync(string.Format(HeroConstants.CacheKey.Captcha, uuid), captchaCode);
            return captchaCode;
        }
    }
}
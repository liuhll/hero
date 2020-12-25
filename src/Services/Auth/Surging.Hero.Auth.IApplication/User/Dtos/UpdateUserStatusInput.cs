using Surging.Cloud.System.Intercept;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.User.Dtos
{
    public class UpdateUserStatusInput
    {
        [CacheKey(1)] public long Id { get; set; }

        public Status Status { get; set; }
    }
}
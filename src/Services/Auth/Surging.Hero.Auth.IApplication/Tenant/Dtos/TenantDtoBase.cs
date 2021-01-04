using System.ComponentModel.DataAnnotations;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.Tenant.Dtos
{
    public abstract class TenantDtoBase
    {
        [Required(ErrorMessage = "租户名称不允许为空")]
        [MaxLength(50,ErrorMessage = "租户名称不允许超过50个字")]
        public string Name { get; set; }

        public string Memo { get; set; }
        
    }
}
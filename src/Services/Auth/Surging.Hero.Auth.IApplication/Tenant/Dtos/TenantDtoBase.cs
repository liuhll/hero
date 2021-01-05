using System.ComponentModel.DataAnnotations;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.Tenant.Dtos
{
    public abstract class TenantDtoBase
    {
        /// <summary>
        /// 租户名称
        /// </summary>
        [Required(ErrorMessage = "租户名称不允许为空")]
        [MaxLength(50,ErrorMessage = "租户名称不允许超过50个字")]
        public string Name { get; set; }

        /// <summary>
        /// 租户备注
        /// </summary>
        public string Memo { get; set; }
        
    }
}
using System.ComponentModel.DataAnnotations;

namespace Surging.Hero.Auth.IApplication.Action.Dtos
{
    public class QueryActionInput
    {
        /// <summary>
        /// 主机名称
        /// </summary>
        [Required(ErrorMessage = "主机名称不允许为空")]
        public string ServiceHost { get; set; }

        /// <summary>
        /// 应用服务名称
        /// </summary>
        [Required(ErrorMessage = "应用服务名称不允许为空")]
        public string AppService { get; set; }

        /// <summary>
        /// 应用服务名称
        /// </summary>
        public string Service { get; set; }
    }
}

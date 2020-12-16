using System.ComponentModel.DataAnnotations;

namespace Surging.Hero.Auth.IApplication.Action.Dtos
{
    public class QueryAppServiceInput
    {
        /// <summary>
        ///     主机名称
        /// </summary>
        [Required(ErrorMessage = "主机名称不允许为空")]
        public string ServiceHost { get; set; }

        /// <summary>
        ///     应用服务名称
        /// </summary>
        public string AppService { get; set; }
    }
}
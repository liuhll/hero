using System.ComponentModel.DataAnnotations;

namespace Surging.Hero.Auth.IApplication.Action.Dtos
{
    public class QueryActionInput
    {
        /// <summary>
        /// 主机名称
        /// </summary>
        public string ServiceHost { get; set; }

        /// <summary>
        /// 应用服务名称
        /// </summary>        
        public string AppService { get; set; }

        /// <summary>
        /// 应用服务名称
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// 服务条目Ids
        /// </summary>
        public long[] Ids { get; set; }
    }
}

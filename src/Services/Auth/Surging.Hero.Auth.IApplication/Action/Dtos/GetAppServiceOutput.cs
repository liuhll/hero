namespace Surging.Hero.Auth.IApplication.Action.Dtos
{
    public class GetAppServiceOutput
    {
        /// <summary>
        /// 主机名称
        /// </summary>
        public string ServiceHost { get; set; }

        /// <summary>
        /// 应用服务名称
        /// </summary>
        public string AppService { get; set; }
    }
}

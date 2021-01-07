using System.ComponentModel;

namespace Surging.Hero.BasicData.Domain.Shared.SystemConfigs
{
    public enum NonPermissionOperationStyle
    {
        /// <summary>
        /// 不显示操作按钮
        /// </summary>
        [Description("不显示")]
        Displayed,
        
        /// <summary>
        /// 操作按钮不可用
        /// </summary>
        [Description("不可用")]
        Disabled
    }
}
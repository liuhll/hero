using System.ComponentModel;

namespace Surging.Hero.Auth.Domain.Shared.Permissions
{
    public enum PermissionMold
    {
        [Description("菜单")]
        Menu,

        [Description("操作")]
        Operation
    }
}

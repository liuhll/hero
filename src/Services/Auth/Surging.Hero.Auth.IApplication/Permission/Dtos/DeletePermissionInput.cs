using Surging.Hero.Auth.Domain.Shared.Permissions;

namespace Surging.Hero.Auth.IApplication.Permission.Dtos
{
    public class DeletePermissionInput
    {
        public long Id { get; set; }

        public PermissionMold Mold { get; set; }
    }
}

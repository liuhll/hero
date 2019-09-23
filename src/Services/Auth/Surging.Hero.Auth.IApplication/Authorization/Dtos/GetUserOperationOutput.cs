using Surging.Hero.Auth.Domain.Shared.Operations;

namespace Surging.Hero.Auth.IApplication.Authorization.Dtos
{
    public class GetUserOperationOutput
    {
        public long PermissionId { get; set; }

        public long MenuId { get; set; }

        public string Code { get; set; }

        public int Level { get; set; }

        public string Icon { get; set; }

        public string Name { get; set; }

        public OperationMold Mold { get; set; }

        public string Memo { get; set; }
    }
}

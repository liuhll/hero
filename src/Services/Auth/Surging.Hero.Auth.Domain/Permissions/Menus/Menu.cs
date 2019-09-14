using Surging.Core.Domain.Entities.Auditing;
using Surging.Hero.Auth.Domain.Shared.Menus;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.Domain.Permissions.Menus
{
    public class Menu : FullAuditedEntity<long>
    {
        public long PermissionId { get; set; }

        public long ParentId { get; set; }

        public string Code { get; set; }

        public int Level { get; set; }

        public string Name { get; set; }

        public string Anchor { get; set; }

        public MenuMold Mold { get; set; }

        public string Icon { get; set; }

        public string FrontEndComponent { get; set; }

        public int Sort { get; set; }

        public string Memo { get; set; }

        public Status Status { get; set; }

    }
}

using Surging.Cloud.Domain.Entities.Auditing;
using Surging.Hero.Auth.Domain.Shared.Menus;

namespace Surging.Hero.Auth.Domain.Permissions.Menus
{
    public class Menu : FullAuditedEntity<long>
    {
        public long PermissionId { get; set; }

        public long ParentId { get; set; }

        public string Code { get; set; }

        public int Level { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public string Path { get; set; }

        public MenuMold Mold { get; set; }

        public string Icon { get; set; }

        public bool AlwaysShow { get; set; } = true;

        public string Component { get; set; }

        public int Sort { get; set; }

        public string Memo { get; set; }
    }
}
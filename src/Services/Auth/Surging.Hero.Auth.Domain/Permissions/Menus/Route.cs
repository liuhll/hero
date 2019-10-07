using Surging.Hero.Common;
using System.Collections.Generic;

namespace Surging.Hero.Auth.Domain.Permissions.Menus
{
    public class Route
    {
        public long PermissionId { get; set; }

        public long ParentId { get; set; }

        public string Name { get; set; }

        public string Path { get; set; }

        public string Title { get; set; }

        public string Icon { get; set; }

        public bool AlwaysShow { get; set; }

        public string Component { get; set; }

        public Status Status { get; set; }

        public IEnumerable<long> RoldIds { get; set; }
    }
}

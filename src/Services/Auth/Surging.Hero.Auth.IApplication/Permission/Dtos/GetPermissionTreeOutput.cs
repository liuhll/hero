using System.Collections.Generic;
using Surging.Core.Domain;
using Surging.Hero.Auth.Domain.Shared.Permissions;

namespace Surging.Hero.Auth.IApplication.Permission.Dtos
{
    public class GetPermissionTreeOutput
    {
        public GetPermissionTreeOutput() {
            Children = new List<GetPermissionTreeOutput>();
        }
        public long Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

        public long ParentId { get; set; }
        public string FullName { get; set; }
        public IEnumerable<GetPermissionTreeOutput> Children { get; set; }

        public PermissionMold Mold { get; set; }
    }
}

using System.Collections.Generic;
using System.Linq;
using Surging.Hero.Auth.Domain.Shared.Permissions;
using Surging.Hero.Common;

namespace Surging.Hero.Auth.IApplication.Role.Dtos
{
    public class GetRolePermissionTreeOutput
    {
        private CheckStatus _checkStatus;

        public GetRolePermissionTreeOutput()
        {
            Children = new List<GetRolePermissionTreeOutput>();
        }

        public long Id { get; set; }

        public long PermissionId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public CheckStatus CheckStatus
        {
            get
            {
                if (Children.Any())
                {
                    if (Children.All(p => p.CheckStatus == CheckStatus.Checked))
                        return CheckStatus.Checked;
                    if (Children.All(p => p.CheckStatus == CheckStatus.UnChecked)) return CheckStatus.UnChecked;
                    return CheckStatus.Indeterminate;
                }

                return _checkStatus;
            }
            set => _checkStatus = value;
        }

        public PermissionMold PermissionMold { get; set; }

        public IEnumerable<GetRolePermissionTreeOutput> Children { get; set; }
    }
}
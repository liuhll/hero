using Surging.Core.CPlatform.Exceptions;
using Surging.Hero.Auth.Domain.Shared.Permissions;
using Surging.Hero.Common;
using System.Collections.Generic;
using System.Linq;

namespace Surging.Hero.Auth.IApplication.Role.Dtos
{
    public class GetRolePermissionTreeOutput
    {
        public GetRolePermissionTreeOutput()
        {
            Children = new List<GetRolePermissionTreeOutput>();
        }

        public long Id { get; set; }

        public long PermissionId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        private CheckStatus _checkStatus;
        public CheckStatus CheckStatus
        {
            get
            {
                if (Children.Any())
                {
                    if (Children.All(p => p.CheckStatus == CheckStatus.Checked))
                    {
                        return CheckStatus.Checked;
                    }
                    else if (Children.All(p => p.CheckStatus == CheckStatus.UnChecked)) {
                        return CheckStatus.UnChecked;
                    }
                    return CheckStatus.Indeterminate;
                }
                return _checkStatus;
                
            }
            set
            {
                _checkStatus = value;
            }
            
        }

        public PermissionMold PermissionMold { get; set; }

        public IEnumerable<GetRolePermissionTreeOutput> Children { get; set; }

        
    }
}

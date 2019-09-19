using Surging.Core.CPlatform.Exceptions;
using Surging.Hero.Auth.Domain.Shared.Permissions;
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

        private bool isChecked;
        public bool IsChecked
        {
            get
            {
                if (Children.Any())
                {
                    return Children.All(p => p.IsChecked);
                }
                return isChecked;
            }
            set
            {
                isChecked = value;
            }
            
        }

        public PermissionMold PermissionMold { get; set; }

        public IEnumerable<GetRolePermissionTreeOutput> Children { get; set; }

        
    }
}

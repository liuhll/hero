using System.Collections.Generic;
using System.Linq;
using Surging.Core.Domain.Trees;
using Surging.Hero.Organization.Domain.Shared.Organizations;

namespace Surging.Hero.Organization.IApplication.Organization.Dtos
{
    public class GetOrganizationTreeOutput : ITree<GetOrganizationTreeOutput>
    {
        private IEnumerable<ITree<GetOrganizationTreeOutput>> _children;

        public OrganizationType OrgType { get; set; }

        public string Title => Name;

        public bool Expand => true;

        public long Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public long ParentId { get; set; }
        public string FullName { get; set; }

        public IEnumerable<ITree<GetOrganizationTreeOutput>> Children
        {
            get
            {
                if (_children == null || !_children.Any()) return null;
                return _children;
            }
            set => _children = value;
        }
    }
}
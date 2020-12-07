using Surging.Core.Domain;
using Surging.Core.Domain.Trees;
using Surging.Hero.Organization.Domain.Shared.Organizations;
using System.Collections.Generic;
using System.Linq;

namespace Surging.Hero.Organization.IApplication.Organization.Dtos
{
    public class GetOrganizationTreeOutput : ITree<GetOrganizationTreeOutput>
    {

        public long Id { get; set; }

        public OrganizationType OrgType { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Title { get { return Name; } }

        public bool Expand { get { return true; } }

        public long ParentId { get; set; }
        public string FullName { get; set; }

        private IEnumerable<ITree<GetOrganizationTreeOutput>> _children;
        public IEnumerable<ITree<GetOrganizationTreeOutput>> Children { get { if (_children == null || !_children.Any()) { return null; } return _children; } set { _children = value; } }
    }
}

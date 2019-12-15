using Surging.Core.Domain;
using Surging.Core.Domain.Trees;
using System.Collections.Generic;

namespace Surging.Hero.Auth.IApplication.UserGroup.Dtos
{
    public class GetUserGroupTreeOutput : ITree<GetUserGroupTreeOutput>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long ParentId { get; set; }
        public string FullName { get; set; }
        public IEnumerable<ITree<GetUserGroupTreeOutput>> Children { get; set; }
    }
}

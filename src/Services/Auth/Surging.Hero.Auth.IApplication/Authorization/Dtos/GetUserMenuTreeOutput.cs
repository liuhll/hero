using System.Collections.Generic;
using Surging.Cloud.Domain.Trees;

namespace Surging.Hero.Auth.IApplication.Authorization.Dtos
{
    public class GetUserMenuTreeOutput : ITree<GetUserMenuTreeOutput>
    {
        public string Path { get; set; }

        public bool AlwaysShow { get; set; }

        public string Title { get; set; }

        public string Icon { get; set; }

        public int Level { get; set; }
        public long Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public long ParentId { get; set; }
        public string FullName { get; set; }
        public IEnumerable<ITree<GetUserMenuTreeOutput>> Children { get; set; }
    }
}
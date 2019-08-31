using Surging.Hero.Organization.IApplication.Position.Dtos;
using System.Collections.Generic;

namespace Surging.Hero.Organization.IApplication.Department.Dtos
{
    public class GetDepartmentOutput : DepartmentDtoBase
    {
        public long Id { get; set; }

        public IEnumerable<GetPositionOutput> Postions { get; set; }
    }
}

using System.Collections.Generic;
using Surging.Hero.Organization.IApplication.Position.Dtos;

namespace Surging.Hero.Organization.IApplication.Department.Dtos
{
    public class CreateDepartmentInput : DepartmentDtoBase
    {
        public long ParentId { get; set; }

        //public long OrgId { get; set; }

        public IEnumerable<CreateOrUpdatePositionInput> Positions { get; set; }
    }
}
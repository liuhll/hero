using System.Collections.Generic;
using Surging.Hero.Organization.IApplication.Position.Dtos;

namespace Surging.Hero.Organization.IApplication.Department.Dtos
{
    public class UpdateDepartmentInput : DepartmentDtoBase
    {
        public long Id { get; set; }

        public IEnumerable<CreateOrUpdatePositionInput> Positions { get; set; }
    }
}
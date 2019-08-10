using Surging.Hero.Organization.IApplication.Position.Dtos;
using System.Collections.Generic;

namespace Surging.Hero.Organization.IApplication.Department.Dtos
{
    public class CreateDepartmentInput : DepartmentDtoBase
    {
        public IEnumerable<CreatePositionInput> Postions { get; set; }
    }
}

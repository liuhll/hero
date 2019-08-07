using Surging.Hero.Common;
using Surging.Hero.Organization.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Surging.Hero.Organization.IApplication.Corporation.Dtos
{
    public class CreateCorporationInput : CorporationDtoBase
    {
        [RegularExpression(RegExpConstants.CorporatioCode, ErrorMessage = "字典类型编码格式不正确")]
        public string Code { get; set; }

        public long ParentId { get; set; }
    }
}


using Surging.Core.Domain.Entities.Auditing;
using Surging.Hero.Common;
using System;

namespace Surging.Hero.Auth.Domain.Permissions.Actions
{
    public class Action : AuditedEntity<long>
    {
        public string ServiceId { get; set; }
        public string ServiceHost { get; set; }
        public string Application { get; set; }
        public string Name { get; set; }

        public string WebApi { get; set; }

        public bool DisableNetwork { get; set; }

        public bool EnableAuthorization { get; set; }

        public bool AllowPermission { get; set; }

        public string Developer { get; set; }

        public DateTime? Date { get; set; }

        public Status Status { get; set; }
    }
}

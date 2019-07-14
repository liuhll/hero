using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Core.CPlatform.Runtime.Session
{
    public class NullSurgingSession : SurgingSessionBase
    {
        private NullSurgingSession()
        {
        }

        public static ISurgingSession Instance { get; } = new RpcContextSession();

        public override long? UserId { get; } = Instance.UserId;
        public override string UserName { get; } = Instance.UserName;
    }
}

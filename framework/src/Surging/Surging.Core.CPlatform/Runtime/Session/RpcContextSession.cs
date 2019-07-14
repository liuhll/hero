using Newtonsoft.Json;
using Surging.Core.CPlatform.Transport.Implementation;

namespace Surging.Core.CPlatform.Runtime.Session
{
    public class RpcContextSession : SurgingSessionBase
    {
        private const string PayloadKey = "payload";

        internal RpcContextSession()
        {
        }

        public override long? UserId
        {
            get
            {
                object payload = RpcContext.GetContext().GetAttachment(PayloadKey);
                if (payload != null)
                {
                    if (payload.GetType() == typeof(string))
                    {
                        dynamic payloadJObject = JsonConvert.DeserializeObject(payload.ToString());
                        return payloadJObject.userId ?? payloadJObject.UserId;
                    }
                    else
                    {
                        dynamic payloadJObject = payload;
                        return payloadJObject.userId ?? payloadJObject.UserId;
                    }
                    
                    
                }
                return null;
            }
        }

        public override string UserName
        {
            get
            {
                object payload = RpcContext.GetContext().GetAttachment(PayloadKey);
                if (payload != null)
                {
                    if (payload.GetType() == typeof(string))
                    {
                        dynamic payloadJObject = JsonConvert.DeserializeObject(payload.ToString());
                        return payloadJObject.userName ?? payloadJObject.UserName;
                    }
                    else
                    {
                        dynamic payloadJObject = payload;
                        return payloadJObject.userName ?? payloadJObject.UserName;
                    }


                }
                return null;
            }
        }
    }
}

using Jose;
using System;
using System.Collections.Generic;
using System.Text;

namespace Surging.Core.ApiGateWay.OAuth.Implementation.Configurations
{
    public class JwtConfig
    {
        public string Iss { get; set; } = "surging";

        public string Aud { get; set; } = "surging_aud";

        public int Period { get; set; } = 1000;

        public JwsAlgorithm EncryptionAlgorithm { get; set; } = JwsAlgorithm.HS256;

        public string SecretKey { get; set; }
    }
}

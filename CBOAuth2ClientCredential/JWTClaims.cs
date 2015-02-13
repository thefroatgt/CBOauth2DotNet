using System;
using System.Collections.Generic;

namespace CBOAuthJWT
{
    
    class JWTClaims
    {
        private string issuer;
        private string subject;
        private string audience;
        private long expiration;

        public void SetIssuer(string iss)
        {
            issuer = iss;
        }

        public void SetSubject(string sub)
        {
            subject = sub;
        }

        public void SetAudience(string aud)
        {
            audience = aud;
        }
        
        public void SetExpiration(long epochTime)
        {
            expiration = epochTime;
        }

        public JWTClaims(string iss, string sub, string aud, long epochTime)
        {
            issuer = iss;
            subject = sub;
            audience = aud;
            expiration = epochTime;
        }

        public Dictionary<String, String> ToJSONString()
        {
            return new Dictionary<string, string>() {
                { "iss", issuer },
                { "sub", subject },
                { "aud", audience},
                { "exp", expiration.ToString()}
            };
        }

    }
}

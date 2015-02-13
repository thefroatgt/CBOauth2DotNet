using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;

namespace CBOAuthJWT
{
    public class CBOAuth2Token
    {
        private string accessToken;
        private string tokenType;
        private string errors;
        private DateTime expiration;

        public string AccessToken
        {
            get{return accessToken;}
        }
        public string TokenType
        {
            get{return tokenType;}
        }
        public string Errors
        {
            get { return errors; }
        }
        public DateTime Expiration
        {
            get { return expiration;}
        }

        public CBOAuth2Token(string jsonToken)
        {
            var jsonSerializer = new JavaScriptSerializer();
            var jsonData = jsonSerializer.Deserialize<Dictionary<string, string>>(jsonToken);

            if (jsonData.Keys.Count > 0)
            {
                string expires_in;
                jsonData.TryGetValue("access_token", out accessToken);
                jsonData.TryGetValue("token_type", out tokenType);
                jsonData.TryGetValue("expires_in", out expires_in);
                jsonData.TryGetValue("error", out errors);
                
                expiration = DateTime.Now.AddSeconds(Convert.ToInt32(expires_in)); 
            }            
        }

        public bool ExpiresSoon()
        {
            long experationTime = this.expiration.Ticks / TimeSpan.TicksPerMillisecond;
            long bufferTime = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) + 5000;
            return experationTime < bufferTime;
        }
    }
}

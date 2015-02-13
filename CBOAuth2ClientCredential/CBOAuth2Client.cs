using System;
using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CBOAuthJWT
{
    public class CBOAuth2Client
    {
        private string ClientID;
        private string Signature;
        private string EnvironmentURI;
        private string Environment;
        public CBOAuth2Token AccessToken;

        private static Dictionary<string, CBOAuth2Token> tokens = new Dictionary<string, CBOAuth2Token>();

        public CBOAuth2Client(string clientID, string signature, string environment)
        {
            ClientID = clientID;
            Signature = signature;
            Environment = environment;
            if (environment.ToLower() == "test")
            {
                EnvironmentURI = "https://wwwtest.careerbuilder.com/share/oauth2/token.aspx";
            }
            else
            {
                EnvironmentURI = "https://www.careerbuilder.com/share/oauth2/token.aspx";
            }
        }


        public CBOAuth2Token GetAccessToken()
        {
            string key = GetTokenKey(ClientID, Signature, Environment);
            CBOAuth2Token token;
            if (tokens.TryGetValue(key, out token) && !token.ExpiresSoon())
            {
                return token;
            }
            else
            {
                {
                    return GetFreshToken(ClientID, Signature, Environment);
                }
            }
        }

        public static void ClearTokenCache()
        {
            tokens.Clear();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private static CBOAuth2Token GetFreshToken(string clientID, string signature,string environment)
        {
            CBOAuth2Client client = new CBOAuth2Client(clientID, signature, environment);
            string key = GetTokenKey(clientID, signature, environment);
            if (tokens.ContainsKey(key) && !tokens[key].ExpiresSoon())
            {
                return tokens[key];
            }
            else
            {
                tokens.Remove(key);
                CBOAuth2Token freshToken = new CBOAuth2Token(client.SendTokenRequest());
                tokens.Add(key, freshToken);
                return freshToken;
            }
        }

        private static string GetTokenKey(string clientID, string signature, string environmentURI)
        {
            return clientID + ":" + signature + ":" + environmentURI;
        }

        private string SendTokenRequest()
        {
            string responseFromServer = "";
            WebResponse response = null;
            try
            {
                WebRequest AuthRequest = WebRequest.Create(EnvironmentURI);
                AuthRequest.Method = "POST";
                AuthRequest.Timeout = 30000;

                byte[] byteArray = Encoding.UTF8.GetBytes(getJWTData());
                AuthRequest.ContentType = "application/x-www-form-urlencoded";
                AuthRequest.ContentLength = byteArray.Length;

                using (Stream sendDataStream = AuthRequest.GetRequestStream())
                {
                    sendDataStream.Write(byteArray, 0, byteArray.Length);
                }

                response = AuthRequest.GetResponse();
                using (StreamReader responseReader = new StreamReader(response.GetResponseStream()))
                {
                    responseFromServer = responseReader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse)
                {
                    using (StreamReader responseReader = new StreamReader(((HttpWebResponse)ex.Response).GetResponseStream()))
                    {
                        throw new WebException(responseFromServer = responseReader.ReadToEnd());
                    }
                }
                ex.Response.Close();
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }
            return responseFromServer;
        }

        private string getJWTData()
        {
            return "grant_type=client_credentials&client_assertion_type=urn:ietf:params:oauth:client-assertion-type:jwt-bearer&client_assertion=" + GetJWT() + "&client_id=" + ClientID;
        }

        public string GetJWT()
        {

            JWTClaims claims = new JWTClaims(ClientID, ClientID, "http://www.careerbuilder.com/share/oauth2/token.aspx", GetExpiration());
            return JsonWebToken.Encode(claims.ToJSONString(), Signature, JwtHashAlgorithm.HS512);
        }

        private int GetExpiration()
        {
            DateTime dt1970 = new DateTime(1970, 1, 1);
            DateTime expDT = DateTime.UtcNow.AddMinutes(1);
            TimeSpan span = expDT - dt1970;
            return Convert.ToInt32(span.TotalSeconds);
        }
    }
}
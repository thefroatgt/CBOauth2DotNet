using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CBOAuthJWT;

namespace CBOAuth2ClientCredentialTests
{
    [TestClass]
    public class JWTTests
    {
        [TestMethod]
        public void EncodeTest()
        {
            string secretKey = "SecretSignature";
            var payload = new Dictionary<string, object>() {
                { "claim1", "FirstClaim" },
                { "claim2", 22222 }
            };

            Assert.AreEqual("eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiJ9.eyJjbGFpbTEiOiJGaXJzdENsYWltIiwiY2xhaW0yIjoyMjIyMn0.RF6NnJp7I4FTDhodpd-RqALqNlADVHpdRA1gaa1RNMG6UBT8ksNru1iCaiNtNv7aJd0j92oLKjYxkIaqoE8jIg", 
                JsonWebToken.Encode(payload, secretKey, JwtHashAlgorithm.HS512));
        }

        [TestMethod]
        public void DecodeTest()
        {
            string jwt = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiJ9.eyJjbGFpbTMiOiJGb3VydGhDbGFpbSIsImNsYWltNCI6NDQ0NDQ0fQ.oim65L5RKDZwza0BKxbyMWcLmstiNdK_Gjr7IhDRUsc3BZHUyIioPks7Kbhl7olE5EQvnU1gc81D3GH0TDq6vw";
            string secretKey = "SecretSignature";

            string jsonPayload = JsonWebToken.Decode(jwt, secretKey);
            
            Assert.AreEqual("{\"claim3\":\"FourthClaim\",\"claim4\":444444}", jsonPayload);

        }

        [TestMethod]
        public void DecodeWrongKey()
        {
            string jwt = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzUxMiJ9.eyJjbGFpbTMiOiJGb3VydGhDbGFpbSIsImNsYWltNCI6NDQ0NDQ0fQ.oim65L5RKDZwza0BKxbyMWcLmstiNdK_Gjr7IhDRUsc3BZHUyIioPks7Kbhl7olE5EQvnU1gc81D3GH0TDq6vw";
            string secretKey = "NotMyKey";

            try
            {
                string jsonPayload = JsonWebToken.Decode(jwt, secretKey);
            }
            catch (SignatureVerificationException ex)
            {
                Assert.IsTrue(ex.Message.StartsWith("Invalid signature"));
            }

        }
    }
}

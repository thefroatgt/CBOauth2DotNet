using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CBOAuthJWT;

namespace CBOAuth2ClientCredentialTests
{
    [TestClass]
    public class CBOAuth2ClientTests
    {
        [TestMethod]
        public void TestJWTCreation()
        {
            CBOAuth2Client TestClient = new CBOAuth2Client("12345", "secretSignature", "test");
            string testJWT = TestClient.GetJWT();

            Assert.IsNotNull(testJWT);
            string[] jwtParts = testJWT.Split('.');
            Assert.AreEqual(3, jwtParts.Length);
        }
    }
}

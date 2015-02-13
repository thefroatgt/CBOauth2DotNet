CBOAuth2ClientCredential_PlatformSoftware
=========================================

This is a .Net client for CareerBuilders Client Credentials OAuth flow.


##Usage##
Include the namespace `CBOAuthJWT`

The constructor requires the following parameters.
+ ClientID - provided by Platform Software
+ Signature - provided by Platform Software
+ Environment - "test" or "production". **Note:** ClientIDs are environment specific. A test client cannot be used in production and vice versa.


Call `GetAccessToken()` which returns a CBOAuth2Token which contains the Access Token used for making OAuth calls and its expiration.

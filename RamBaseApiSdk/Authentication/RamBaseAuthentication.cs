using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RamBase.Api.Sdk.Authentication
{
    internal class RamBaseAuthentication
    {
        public string Authenticate = "oauth2";
        public string AccessTokenRetrieval = "oauth2/access_token";

        private HttpClient _httpClient;
        private string _PKCECodeVerifier;

        /// <summary>
        /// Used for authenticating with RamBase API server
        /// </summary>
        /// <param name="httpClient">Http client</param>
        public RamBaseAuthentication(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        #region Resource owner password flow 

        /// <summary>
        /// Get access token and refresh token using Resource Owner Password Flow
        /// </summary>
        /// <param name="clientId">RamBase client id</param>
        /// <param name="clientSecret">RamBase client secret</param>
        /// <param name="username">RamBase user name</param>
        /// <param name="password">RamBase password</param>
        /// <param name="otp">One Time Password</param>
        /// <param name="target">Login target</param>
        /// <param name="forwardedIp">IP address of end user</param>
        /// <returns>Task with LoginResponse containing login info</returns>
        /// <exception cref="LoginException">When login fails</exception>
        public Task<LoginResponse> ResourceOwnerPasswordFlowAsync(
            string clientId,
            string clientSecret,
            string username,
            string password,
            string otp,
            string target,
            string forwardedIp
            )
        {
            string uri = AccessTokenRetrieval;

            string postData = string.Format(
                    "grant_type={0}&client_id={1}&client_secret={2}&username={3}&password={4}",
                    "password",
                    clientId,
                    clientSecret,
                    HttpUtility.UrlEncode(username),
                    HttpUtility.UrlEncode(password)
                );

            if (!string.IsNullOrEmpty(otp))
                postData = string.Format("{0}&otp={1}", postData, otp);

            if (!string.IsNullOrEmpty(target))
                postData = string.Format("{0}&target={1}", postData, target);

            if (!string.IsNullOrEmpty(forwardedIp))
                postData = string.Format("{0}&endclientip={1}", postData, forwardedIp);

            return PerformAuthentication(uri, postData);
        }

        #endregion

        #region Server-side web application flow

        /// <summary>
        /// Get the uri used in Server-Side Application Flow  
        /// </summary>
        /// <param name="domain">Authentication server domain</param>
        /// <param name="clientId">RamBase client id</param>
        /// <param name="redirectUri">A registered redirect_uri for that client ID</param>
        /// <param name="state">Any string that your application would use to maintain state between the request and redirect response. Your application will receive the same value that it sends for this parameter. For example, you could use this parameter to redirect the user to a particular resource in your application</param>
        /// <param name="usePKCE"></param>
        /// <returns>Authentication link for server-side application flow</returns>
        public string GetServerSideApplicationLink(string domain, string clientId, string redirectUri, string state, bool usePKCE)
        {
            string authenticationLink = GetAuthenticationLink(domain, clientId, redirectUri, "code", state);
            if (usePKCE)
            {
                _PKCECodeVerifier = RandomDataBase64url(32);
                string PKCECodeChallenge = Base64urlencodeNoPadding(Sha256(_PKCECodeVerifier));
                authenticationLink = string.Format("{0}{1}{2}{3}", authenticationLink, "&code_challenge=", PKCECodeChallenge, "&code_challenge_method=S256");
            }
            return authenticationLink;
        }

        /// <summary>
        /// Exchange the oauth authorization code for the access token and a refresh token
        /// </summary>
        /// <param name="code">Oauth authorization code</param>
        /// <param name="clientId">RamBase clien id</param>
        /// <param name="clientSecret">RamBase client secret</param>
        /// <param name="redirectUri">A registered redirected_uri for that client id</param>
        /// <returns>Task with LoginResponse containing access token and refresh token</returns>
        public async Task<LoginResponse> GetAccessTokenFromOauthCode(string code, string clientId, string clientSecret, string redirectUri)
        {
            string postData = string.Format(
                    "grant_type={0}&client_id={1}&client_secret={2}&code={3}&redirect_uri={4}",
                    "authorization_code",
                    clientId,
                    clientSecret,
                    code,
                    redirectUri
               );

            if (!string.IsNullOrEmpty(_PKCECodeVerifier))
                postData += "&code_verifier=" + _PKCECodeVerifier;

            LoginResponse loginResponse = await PerformAuthentication(AccessTokenRetrieval, postData);
            _PKCECodeVerifier = "";
            return loginResponse;
        }
        #endregion

        #region Client-side web application flow
        /// <summary>
        /// Get link used in Client-Side Application Flow
        /// </summary>
        /// <param name="clientId">RamBase client id</param>
        /// <param name="redirectUri">A registered redirect_uri for that client id</param>
        /// <param name="state">Any string that your application would use to maintain state between the request and redirect response. Your application will receive the same value that it sends for this parameter. For example, you could use this parameter to redirect the user to a particular resource in your application</param>
        /// <returns>Authentication link for client-side application flow</returns>
        public string GetClientSideAuthenticationLink(string domain, string clientId, string redirectUri, string state)
        {
            return GetAuthenticationLink(domain, clientId, redirectUri, "token", state);
        }

        #endregion

        #region Client credentials flow
        /// <summary>
        /// Get an access token with client cretentials flow
        /// </summary>
        /// <param name="clientId">RamBase client id</param>
        /// <param name="clientSecret">RamBase client secret</param>
        /// <param name="grantType">Set this value to client_credentials</param>
        /// <param name="customerId">The customer you want this login to be associated with</param>
        /// <param name="supplierId">The supplier you want this login to be associated with</param>
        /// <param name="target">The target system you want to login to. Can be left blank if the client only has access to one target system</param>
        /// <param name="endClientIp">If you perform a login where you need to forward the Ip address of the end user, this value will override the client ip address. This will only be necessary if the ip of the server differs from the ip of the end user. This will only work if the host ip is within a trusted ip range</param>
        /// <returns>Task with a LoginResponse containing access token</returns>
        public Task<LoginResponse> ClientCredentialsFlowAsync(
                string clientId,
                string clientSecret,
                string grantType,
                string customerId,
                string supplierId,
                string target,
                string endClientIp
            )
        {
            string uri = AccessTokenRetrieval;
            string postData = string.Format("client_id={0}&client_secret={1}&grant_type={2}", clientId, clientSecret, grantType);

            if (!string.IsNullOrEmpty(customerId))
                postData = string.Format("{0}&customerid={1}", postData, customerId);

            if (!string.IsNullOrEmpty(supplierId))
                postData = string.Format("{0}&supplierid={1}", postData, supplierId);

            if(!string.IsNullOrEmpty(target))
                postData = string.Format("{0}&target={1}", postData, target);

            if (!string.IsNullOrEmpty(endClientIp))
                postData = string.Format("{0}&endclientip={1}", postData, endClientIp);

            return PerformAuthentication(uri, postData);
        }

        #endregion

        #region Refresh
        /// <summary>
        /// Exchange the refresh token for the access token
        /// </summary>
        /// <param name="clientId">RamBase client id</param>
        /// <param name="clientSecret">RamBase client secret</param>
        /// <param name="refreshToken">The refresh token</param>
        /// <returns>Task with LoginResponse containing accesstoken</returns>
        public Task<LoginResponse> RefreshLogin(string clientId, string clientSecret, string refreshToken)
        {
            string uri = AccessTokenRetrieval;
            string postData = string.Format(
                    "client_id={0}&client_secret={1}&refresh_token={2}&grant_type={3}",
                    clientId,
                    clientSecret,
                    refreshToken,
                    "refresh_token"
                );
            return PerformAuthentication(uri, postData);
        }

        #endregion

        #region Common
        /// <summary>
        /// Creates url for authenticating with a web browser
        /// </summary>
        /// <param name="clientId">RamBase client id</param>
        /// <param name="redirectUri">A registered redirect_uri for your client ID</param>
        /// <param name="responseType"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private string GetAuthenticationLink(string domain, string clientId, string redirectUri, string responseType, string state)
        {
            return string.Format("{0}{1}?client_id={2}&response_type={3}&redirect_uri={4}&state={5}", domain, Authenticate, clientId, responseType, redirectUri, state);
        }

        /// <summary>
        /// Performs authentication request
        /// </summary>
        /// <param name="uri">Authentication uri</param>
        /// <param name="postData">x-www-form-urlencoded formatted authentication info</param>
        /// <returns>Task with LoginResponse</returns>
        private async Task<LoginResponse> PerformAuthentication(string uri, string postData)
        {
            StringContent content = new StringContent(postData, Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await _httpClient.PostAsync(uri, content);
            await VerifyLogin(response);
            return await CreateLoginResponse(response);
        }

        /// <summary>
        /// Verifies login response. Checks HTTP status code and content error_code
        /// </summary>
        /// <param name="response">HTTP response message to verify</param>
        /// <returns>Empty task</returns>
        /// <exception cref="LoginException">When http status code is not successful</exception>
        /// <exception cref="OtpRequiredException">When OTP is required</exception>
        /// <exception cref="TargetRequiredException">When target is required</exception>
        private async Task VerifyLogin(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                JsonLoginError loginError = JsonConvert.DeserializeObject<JsonLoginError>(errorResponse);

                if (loginError == null)
                    throw new LoginException(response);

                if (loginError.error_code == 5)
                {
                    throw new OtpRequiredException(loginError.targets, loginError.otp_method);
                }
                else if (loginError.error_code == 10)
                {
                    throw new TargetRequiredException(loginError.targets);
                }
            }

            throw new LoginException(response);
        }

        /// <summary>
        /// Creates LoginResponse based on HTTP response message
        /// </summary>
        /// <param name="response">HTTP response message containing access token</param>
        /// <returns>LoginResponse</returns>
        private async Task<LoginResponse> CreateLoginResponse(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            JsonLogin loginResult = JsonConvert.DeserializeObject<JsonLogin>(
                json,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }
            );

            DateTime expire = loginResult.expires_in != null ?
            DateTime.Now.AddSeconds(double.Parse(loginResult.expires_in.Replace("\"", "").Replace("}", "")))
            : DateTime.Now;

            return new LoginResponse(loginResult.access_token, loginResult.refresh_token, expire, loginResult.is_target_test_system);
        }
        #endregion

        #region PKCE

        //ref https://github.com/googlesamples/oauth-apps-for-windows

        /// <summary>
        /// Returns URI-safe data with a given input length.
        /// </summary>
        /// <param name="length">Input length (nb. output will be longer)</param>
        /// <returns></returns>
        public static string RandomDataBase64url(uint length)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[length];
            rng.GetBytes(bytes);
            return Base64urlencodeNoPadding(bytes);
        }

        /// <summary>
        /// Returns the SHA256 hash of the input string.
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static byte[] Sha256(string inputString)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(inputString);
            SHA256Managed sha256 = new SHA256Managed();
            return sha256.ComputeHash(bytes);
        }

        /// <summary>
        /// Base64url no-padding encodes the given input buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string Base64urlencodeNoPadding(byte[] buffer)
        {
            string base64 = Convert.ToBase64String(buffer);

            // Converts base64 to base64url.
            base64 = base64.Replace("+", "-");
            base64 = base64.Replace("/", "_");
            // Strips padding.
            base64 = base64.Replace("=", "");

            return base64;
        }

        #endregion
    }
}
using RamBase.Api.Sdk.Access;
using RamBase.Api.Sdk.Authentication;
using RamBase.Api.Sdk.DomainValues;
using RamBase.Api.Sdk.Meta;
using RamBase.Api.Sdk.Operations;
using RamBase.Api.Sdk.Request;
using RamBase.Api.Sdk.Sessions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RamBase.Api.Sdk
{
    public class RamBaseApi
    {
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string ApiPath { get; set; }

        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }
        public DateTime ExpireTime { get; private set; }

        private const int _timeout = 30000;

        private readonly HttpClient _httpClient;

        private readonly RamBaseAuthentication _authentication;
        private readonly RamBaseRequest _request;
        private readonly RamBaseOperations _operations;
        private readonly RamBaseAccess _access;
        private readonly RamBaseDomainValues _domainValues;
        private readonly RamBaseMetadata _metadata;
        private readonly RamBaseSessions _sessions;

        /// <summary>
        /// RamBase API Client
        /// </summary>
        /// <param name="clientId">RamBase client id</param>
        /// <param name="clientSecret">RamBase client secret</param>
        /// <param name="defaultTimeout">Default HTTP request timeout</param>
        /// <param name="defaultPath">Default API path</param>
        public RamBaseApi(string clientId, string clientSecret, int defaultTimeout = _timeout, string defaultPath = "https://api.rambase.net")
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            ApiPath = defaultPath;

            if (!defaultPath.EndsWith("/"))
                ApiPath += "/";

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.Timeout = TimeSpan.FromMilliseconds(defaultTimeout);
            _httpClient.BaseAddress = new Uri(defaultPath);

            _authentication = new RamBaseAuthentication(_httpClient);
            _request = new RamBaseRequest(_httpClient);
            _operations = new RamBaseOperations(_request);
            _access = new RamBaseAccess(_request);
            _domainValues = new RamBaseDomainValues(_request);
            _metadata = new RamBaseMetadata(_request);
            _sessions = new RamBaseSessions(_request);
        }

        /// <summary>
        /// This constructor requires an accessToken provided from elsewhere. It sets the clientId and clientSecret to null, meaning that you won't be able to use the SDKs OAuth methods.
        /// </summary>
        /// <param name="accessToken">RamBase access token</param>
        /// <param name="defaultTimeout">Default HTTP request timeout</param>
        /// <param name="defaultPath">Default API path</param>
        public RamBaseApi(string accessToken, int defaultTimeout = _timeout, string defaultPath = "https://api.rambase.net") : this(null, null, defaultTimeout, defaultPath)
        {
            LoginWithAccessToken(accessToken);
        }

        #region Login

        /// <summary>
        /// Login with an access token
        /// </summary>
        /// <param name="accessToken">RamBase access token</param>
        public void LoginWithAccessToken(string accessToken)
        {
            AccessToken = accessToken;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        /// <summary>
        /// Login with an access token
        /// </summary>
        /// <param name="accessToken">RamBase access token</param>
        /// <param name="expireTime">Time when token expires</param>
        public void LoginWithAccessToken(string accessToken, DateTime expireTime)
        {
            SetLoginInfo(accessToken, null, expireTime);
        }

        /// <summary>
        /// Login to RamBase with Username and Password
        /// </summary>
        /// <param name="username">The username entered by the user</param>
        /// <param name="password">The password entered by the user</param>
        /// <returns>Empty Task</returns>
        /// <exception cref="LoginException">When login fails</exception>
        /// <exception cref="OtpRequiredException">When OTP is required</exception>
        public async Task LoginAsync(string username, string password)
        {
            LoginResponse loginResponse = await _authentication.ResourceOwnerPasswordFlowAsync(ClientId, ClientSecret, username, password, "", "", "");
            SetLoginInfo(loginResponse.AccessToken, loginResponse.RefreshToken, loginResponse.ExpireTime);
        }

        /// <summary>
        /// Login to RamBase with Username, Password [, One Time Password, target and forwarded ip]
        /// </summary>
        /// <param name="username">The username entered by the user</param>
        /// <param name="password">The password entered by the user</param>
        /// <param name="otp">One time password</param>
        /// <param name="target">Target system</param>
        /// <param name="forwardedIp">Forwarded ip</param>
        /// <returns></returns>
        public async Task LoginAsync(string username, string password, string target = "", string otp = "", string forwardedIp = "")
        {
            LoginResponse loginResponse = await _authentication.ResourceOwnerPasswordFlowAsync(ClientId, ClientSecret, username, password, otp, target, forwardedIp);
            SetLoginInfo(loginResponse.AccessToken, loginResponse.RefreshToken, loginResponse.ExpireTime);
        }

        /// <summary>
        /// Login to RamBase with Username, Password and One Time Password
        /// The otp will be sent after trying to login normally (see LoginAsync)
        /// </summary>
        /// <param name="username">The username entered by the user</param>
        /// <param name="password">The password entered by the user</param>
        /// <param name="otp">If you login using pid and password you will also need a “one time password” (OTP) if you are outside a “safe zone”. The OTP number is sent as an email or SMS to the user, when it is required</param>
        /// <param name="target">This is in most cases optional. Use this parameter to specify RamBase target (eg. HATTELAND, JHCDEVSYS). If not provided and the user only have access to one target, the target will be automatically set. If the user has access to multiple targets, you will get 401 Unauthorized and a list of allowed targets</param>
        /// <param name="forwardedIp">If you perform a login where you need to forward the Ip address of the end user, this value will override the client ip address. This will only be necessary if the ip of the server differs from the ip of the end user. This will only work if the host ip is within a trusted ip range</param>
        /// <returns>Empty Task</returns>
        /// <exception cref="LoginException">When login fails</exception>
        public async Task LoginWithOtpAsync(string username, string password, string otp, string target = "", string forwardedIp = "")
        {
            LoginResponse loginResponse = await _authentication.ResourceOwnerPasswordFlowAsync(ClientId, ClientSecret, username, password, otp, target, forwardedIp);
            SetLoginInfo(loginResponse.AccessToken, loginResponse.RefreshToken, loginResponse.ExpireTime);
        }

        /// <summary>
        /// Login to RamBase with Username, Password and Target
        /// </summary>
        /// <param name="username">The username entered by the user</param>
        /// <param name="password">The password entered by the user</param>
        /// <param name="target">Target system</param>
        /// <param name="otp">One time password</param>
        /// <param name="forwardedIp">Forwarded ip</param>
        /// <returns></returns>
        public async Task LoginWithTargetAsync(string username, string password, string target, string otp = "", string forwardedIp = "")
        {
            LoginResponse loginResponse = await _authentication.ResourceOwnerPasswordFlowAsync(ClientId, ClientSecret, username, password, otp, target, forwardedIp);
            SetLoginInfo(loginResponse.AccessToken, loginResponse.RefreshToken, loginResponse.ExpireTime);
        }

        /// <summary>
        /// Server-side web application flow (https://api.rambase.net/gettingstarted/gettingaccess#gettingAccess31)
        /// </summary>
        /// <param name="oauthCode">Oauth Authorization code</param>
        /// <param name="redirectUri">A registered redirect_uri for that client ID</param>
        /// <returns>Empty task</returns>
        /// <exception cref="LoginException">When login fails</exception>
        public async Task LoginWithAuthorizationCodeAsync(string oauthCode, string redirectUri)
        {
            LoginResponse loginResponse = await _authentication.GetAccessTokenFromOauthCode(oauthCode, ClientId, ClientSecret, redirectUri);
            SetLoginInfo(loginResponse.AccessToken, loginResponse.RefreshToken, loginResponse.ExpireTime);
        }

        /// <summary>
        /// Client credentials flow (https://api.rambase.net/gettingstarted/gettingaccess#gettingAccess34)
        /// </summary>
        /// <param name="customerId">The customer you want this login to be associated with</param>
        /// <param name="supplierId">The supplier you want this login to be associated with</param>
        /// <param name="endClientIp">If you perform a login where you need to forward the Ip address of the end user, this value will override the client ip address. This will only be necessary if the ip of the server differs from the ip of the end user. This will only work if the host ip is within a trusted ip range</param>
        /// <returns>Empty task</returns>
        /// <exception cref="LoginException">When login fails</exception>
        public async Task LoginWithClientCredentialsAsync(string customerId = "", string supplierId = "", string endClientIp = "")
        {
            LoginResponse loginResponse = await _authentication.ClientCredentialsFlowAsync(ClientId, ClientSecret, "client_credentials", customerId, supplierId, endClientIp);
            LoginWithAccessToken(loginResponse.AccessToken);
        }

        /// <summary>
        /// Server-side authentication url used with Server-side web application flow (https://api.rambase.net/gettingstarted/gettingaccess#gettingAccess31)
        /// </summary>
        /// <param name="redirectUri">A registered redirect_uri for that client ID.</param>
        /// <param name="state">Any string that your application would use to maintain state between the request and redirect response. Your application will receive the same value that it sends for this parameter. For example, you could use this parameter to redirect the user to a particular resource in your application</param>
        /// <returns>Link to RamBase authentication website</returns>
        public string GetServerSideAuthenticationLink(string redirectUri, string state = "", bool usePKCE = false)
        {
            return _authentication.GetServerSideApplicationLink(ApiPath, ClientId, redirectUri, state, usePKCE);
        }

        /// <summary>
        /// Server-side authentication url used with Server-side web application flow (https://api.rambase.net/gettingstarted/gettingaccess#gettingAccess31)
        /// </summary>
        /// <param name="redirectUri">A registered redirect_uri for that client ID.</param>
        /// <param name="state">Any string that your application would use to maintain state between the request and redirect response. Your application will receive the same value that it sends for this parameter. For example, you could use this parameter to redirect the user to a particular resource in your application</param>
        /// <param name="usePKCE"></param>
        /// <returns>Link to RamBase authentication website</returns>
        public string GetServerSideAuthenticationLink(string redirectUri, bool usePKCE, string state = "")
        {
            return _authentication.GetServerSideApplicationLink(ApiPath, ClientId, redirectUri, state, usePKCE);
        }

        /// <summary>
        /// Client-side authentication url used with Client-side web application flow (https://api.rambase.net/gettingstarted/gettingaccess#gettingAccess32)
        /// </summary>
        /// <param name="redirectUri">A registered redirect_uri for your client ID</param>
        /// <param name="state">Any string that your application would use to maintain state between the request and redirect response. Your application will receive the same value that it sends for this parameter. For example, you could use this parameter to redirect the user to a particular resource in your application</param>
        /// <returns>Link to RamBase authentication website</returns>
        public string GetClientSideAuthenticationLink(string redirectUri, string state = "")
        {
            return _authentication.GetClientSideAuthenticationLink(ApiPath, ClientId, redirectUri, state);
        }

        /// <summary>
        /// Refreshes access token using refresh token
        /// </summary>
        /// <exception cref="LoginException">When refresh token is missing or login fails</exception>
        public async Task RefreshLogin()
        {
            if (string.IsNullOrEmpty(RefreshToken))
            {
                throw new LoginException("Missing refresh token");
            }
            LoginResponse loginResponse = await _authentication.RefreshLogin(ClientId, ClientSecret, RefreshToken);
            SetLoginInfo(loginResponse.AccessToken, loginResponse.RefreshToken, loginResponse.ExpireTime);
        }

        /// <summary>
        /// Asynchronously logs out of RamBase
        /// </summary>
        /// <returns></returns>
        public async Task Logout()
        {
            await _request.PerformRequestAsync(ApiResourceVerb.POST, "oauth2/logout");
            AccessToken = null;
            RefreshToken = null;
            ExpireTime = DateTime.Now;
        }

        public bool IsLoggedIn()
        {
            return (!string.IsNullOrEmpty(AccessToken) && (DateTime.Compare(ExpireTime, DateTime.Now) > 0));
        }

        /// <summary>
        /// Sets login info
        /// </summary>
        /// <param name="accessToken">Oauth access token</param>
        /// <param name="refreshToken">Oauth refresh token</param>
        /// <param name="expireTime">Oauth access token expire time</param>
        private void SetLoginInfo(string accessToken, string refreshToken, DateTime expireTime)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpireTime = expireTime;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        #endregion

        #region Verbs

        /// <summary>
        /// Perform an async GET request
        /// </summary>
        /// <param name="uri">Relative or explicit path to resource</param>
        /// <param name="parameters">Url query parameters</param>
        /// <returns>Task with ApiResponse containing response body as JSON</returns>
        /// <exception cref="RequestException">Whenever a request fails</exception>
        public Task<ApiResponse> GetAsync(string uri, string parameters = "")
        {
            return _request.PerformRequestAsync(ApiResourceVerb.GET, uri, default, parameters);
        }

        /// <summary>
        /// Perform an async GET request
        /// </summary>
        /// <param name="uri">Relative or explicit path to resource</param>
        /// <param name="parameters">Url query parameters</param>
        /// <returns>Task with ApiResponse containing response body as JSON</returns>
        /// <exception cref="RequestException">Whenever a request fails</exception>
        /// <returns></returns>
        public Task<ApiResponse> GetAsync(string uri, GetParameters parameters)
        {
            return _request.PerformRequestAsync(ApiResourceVerb.GET, uri, default, parameters.Build());
        }

        /// <summary>
        /// Perform an async POST request
        /// </summary>
        /// <param name="uri">Relative or explicit path to resource</param>
        /// <param name="data">Request body as JSON</param>
        /// <param name="parameters">Url query parameters</param>
        /// <returns>Task with ApiResponse containing response body as JSON</returns>
        /// <exception cref="RequestException">Whenever a request fails</exception>
        public Task<ApiResponse> PostAsync(string uri, string data, string parameters = "")
        {
            return _request.PerformRequestAsync(ApiResourceVerb.POST, uri, data, parameters);
        }

        /// <summary>
        /// Perform an async POST request
        /// </summary>
        /// <param name="uri">Relative or explicit path to resource</param>
        /// <param name="data">Request body as JSON</param>
        /// <param name="parameters">Url query parameters</param>
        /// <returns>Task with ApiResponse containing response body as JSON</returns>
        /// <exception cref="RequestException">Whenever a request fails</exception>
        public Task<ApiResponse> PostAsync(string uri, string data, PostParameters parameters)
        {
            return _request.PerformRequestAsync(ApiResourceVerb.POST, uri, data, parameters.Build());
        }

        /// <summary>
        /// Perform an async PUT request
        /// </summary>
        /// <param name="uri">Relative or explicit path to resource</param>
        /// <param name="data">Request body as JSON</param>
        /// <param name="parameters">Url query parameters</param>
        /// <returns>Task with ApiResponse containing response body as JSON</returns>
        /// <exception cref="RequestException">Whenever a request fails</exception>
        public Task<ApiResponse> PutAsync(string uri, string data, string parameters = "")
        {
            return _request.PerformRequestAsync(ApiResourceVerb.PUT, uri, data, parameters);
        }

        /// <summary>
        /// Perform an async PUT request
        /// </summary>
        /// <param name="uri">Relative or explicit path to resource</param>
        /// <param name="data">Request body as JSON</param>
        /// <param name="parameters">Url query parameters</param>
        /// <returns>Task with ApiResponse containing response body as JSON</returns>
        /// <exception cref="RequestException">Whenever a request fails</exception>
        public Task<ApiResponse> PutAsync(string uri, string data, PutParameters parameters)
        {
            return _request.PerformRequestAsync(ApiResourceVerb.PUT, uri, data, parameters.Build());
        }

        /// <summary>
        /// Perform an async DELETE request
        /// </summary>
        /// <param name="uri">Relative or explicit path to resource</param>
        /// <param name="parameters">Url query parameters</param>
        /// <returns>Task with ApiResponse containing response body as JSON</returns>
        /// <exception cref="RequestException">Whenever a request fails</exception>
        public Task<ApiResponse> DeleteAsync(string uri, string parameters = "")
        {
            return _request.PerformRequestAsync(ApiResourceVerb.DELETE, uri, default, parameters);
        }

        /// <summary>
        /// Perform an async DELETE request
        /// </summary>
        /// <param name="uri">Relative or explicit path to resource</param>
        /// <param name="parameters">Url query parameters</param>
        /// <returns>Task with ApiResponse containing response body as JSON</returns>
        /// <exception cref="RequestException">Whenever a request fails</exception>
        public Task<ApiResponse> DeleteAsync(string uri, DeleteParameters parameters)
        {
            return _request.PerformRequestAsync(ApiResourceVerb.DELETE, uri, default, parameters.Build());
        }
        #endregion

        #region Operations

        /// <summary>
        /// Asynchronously starts a RamBase operation for the given resource 
        /// </summary>
        /// <param name="operationId">The id of the operation</param>
        /// <param name="resourceUri">Relative path to the resource</param>
        /// <returns>Task with an OperationInstance</returns>
        /// <exception cref="RequestException">Whenever a request fails</exception>
        public Task<OperationInstance> StartOperationAsync(int operationId, string resourceUri)
        {
            return _operations.StartOperationAsync(operationId, resourceUri, default, default);
        }

        /// <summary>
        /// Asynchronously starts a RamBase operation for the given resource 
        /// </summary>
        /// <param name="operationId">The id of the operation</param>
        /// <param name="resourceUri">Relative path to the resource</param>
        /// <param name="parameters">Url query parameters</param>
        /// <returns>Task with an OperationInstance</returns>
        /// <exception cref="RequestException">Whenever a request fails</exception>
        public Task<OperationInstance> StartOperationAsync(int operationId, string resourceUri, string parameters = "")
        {
            return _operations.StartOperationAsync(operationId, resourceUri, default, parameters);
        }

        /// <summary>
        /// Asynchronously starts a RamBase operation for the given resource 
        /// </summary>
        /// <param name="operationId">The id of the operation</param>
        /// <param name="resourceUri">Relative path to the resource</param>
        /// <param name="parameters">Url queryparameters</param>
        /// <returns>Task with an OperationInstance</returns>
        /// <exception cref="RequestException">Whenever a request fails</exception>
        public Task<OperationInstance> StartOperationAsync(int operationId, string resourceUri, OperationParameters parameters)
        {
            return _operations.StartOperationAsync(operationId, resourceUri, default, parameters.Build());
        }


        /// <summary>
        /// Asynchronously starts a RamBase operation for the given resourse
        /// </summary>
        /// <param name="operationId">The id of the operation</param>
        /// <param name="resourceUri">Relative path to the resource</param>
        /// <param name="data">Operation body as JSON</param>
        /// <param name="parameters">Url query parameters</param>
        /// <returns>Task with an OperationInstance</returns>
        /// <exception cref="RequestException">Whenever a request fails</exception>
        public Task<OperationInstance> StartOperationAsync(int operationId, string resourceUri, string data = "", string parameters = "")
        {
            return _operations.StartOperationAsync(operationId, resourceUri, data, parameters);
        }


        /// <summary>
        /// Asynchronously starts a RamBase operation for the given resourse
        /// </summary>
        /// <param name="operationId">The id of the operation</param>
        /// <param name="resourceUri">Relative path to the resource</param>
        /// <param name="data">Operation body as JSON</param>
        /// <param name="parameters">Url query parameters</param>
        /// <returns>Task with an OperationInstance</returns>
        /// <exception cref="RequestException">Whenever a request fails</exception>
        public Task<OperationInstance> StartOperationAsync(int operationId, string resourceUri, OperationParameters parameters, string data = "")
        {
            return _operations.StartOperationAsync(operationId, resourceUri, data, parameters.Build());
        }

        /// <summary>
        /// Get the status of given operation
        /// </summary>
        /// <param name="operation">The operation</param>
        /// <returns>Task with an OperationInstance</returns>
        /// <exception cref="RequestException">Whenever a request fails</exception
        public Task<OperationInstance> GetOperationStatusAsync(OperationInstance operation)
        {
            return _operations.GetOperationInstanceStatusAsync(operation);
        }
        #endregion

        #region Batch

        /// <summary>
        /// Asynchronously performe a batch request
        /// </summary>
        /// <param name="requests">List of paths to resources with Url parameters</param>
        /// <returns>Task with a list of Resources in the order they were requested</returns>
        /// <exception cref="RequestException">Whenever batch request fails</exception>
        public Task<List<Resource>> GetBatchAsync(params string[] requests)
        {
            return _request.GetBatchAsync(new List<string>(requests));
        }

        /// <summary>
        /// Asynchronously performe a batch request
        /// </summary>
        /// <param name="requests">BatchRequest</param>
        /// <returns>Task with a list of Resources in the order they were requested</returns>
        /// <exception cref="RequestException">Whenever batch request fails</exception>
        public Task<List<Resource>> GetBatchAsync(BatchRequest requests)
        {
            return _request.GetBatchAsync(requests.Build());
        }

        #endregion

        #region Metadata

        /// <summary>
        /// Asynchronously get metadata for given resource with given verb
        /// </summary>
        /// <param name="url">Relative or explicit path to resource</param>
        /// <param name="verb">Http verb</param>
        /// <param name="parameters">Url query parameters</param>
        /// <returns>Task with Metadata</returns>
        /// <exception cref="RequestException">Whenever a request fails</exception>
        public Task<Metadata> GetMetadataAsync(string url, ApiResourceVerb verb, string parameters = "")
        {
            return _metadata.GetMetadataAsync(url, verb, parameters);
        }

        /// <summary>
        /// Asynchronously get metadata for given resource with given verb
        /// </summary>
        /// <param name="url">Relative or explicit path to resource</param>
        /// <param name="verb">Http verb</param>
        /// <param name="parameters">Url query parameters</param>
        /// <returns>Task with Metadata</returns>
        /// <exception cref="RequestException">Whenever a request fails</exception>
        public Task<Metadata> GetMetadataAsync(string url, ApiResourceVerb verb, MetadataParameters parameters)
        {
            return _metadata.GetMetadataAsync(url, verb, parameters.Build());
        }
        #endregion

        #region DomainValues

        /// <summary>
        /// Asynchronously get applicable domain values
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="field">Field</param>
        /// <param name="parameters">Url query parameters</param>
        /// <returns>Task with list of DomainValues</returns>
        /// <exception cref="RequestException">Whenever a request fails</exception>
        public Task<List<DomainValue>> GetApplicableDomainValuesAsync(string obj, string field, string parameters = "")
        {
            return _domainValues.GetApplicableDomainValuesAsync(obj, field, parameters);
        }

        /// <summary>
        /// Asynchronously get applicable domain values
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="field">Field</param>
        /// <param name="parameters">Url query parameters</param>
        /// <returns>Task with list of DomainValues</returns>
        /// <exception cref="RequestException">Whenever a request fails</exception>
        public Task<List<DomainValue>> GetApplicableDomainValuesAsync(string obj, string field, DomainValueParameters parameters)
        {
            return _domainValues.GetApplicableDomainValuesAsync(obj, field, parameters.Build());
        }
        #endregion

        #region CheckAccess

        /// <summary>
        /// Asynchronously checks given access checks
        /// </summary>
        /// <param name="checkAccessRequests">Accesses to check</param>
        /// <param name="parameters">Url query parameters</param>
        /// <returns>Task with list of check access results</returns>
        /// <exception cref="RequestException">Whenever a request fails</exception>
        public Task<List<CheckAccessResult>> CheckAccessAsync(CheckAccessRequest checkAccessRequests, string parameters = "")
        {
            return _access.CheckAccessAsync(checkAccessRequests.Requests, parameters);
        }

        /// <summary>
        /// Asynchronously checks given access checks
        /// </summary>
        /// <param name="CheckAccessRequests">Accesses to check</param>
        /// <param name="parameters">Url query parameters</param>
        /// <returns>Task with list of access check results</returns>
        /// <exception cref="RequestException">Whenever a request fails</exception>
        public Task<List<CheckAccessResult>> CheckAccessAsync(CheckAccessRequest CheckAccessRequests, CheckAccessParameters parameters)
        {
            return _access.CheckAccessAsync(CheckAccessRequests.Requests, parameters.Build());
        }

        #endregion

        #region Sessions

        /// <summary>
        /// Asynchronously gets information about the current session
        /// </summary>
        /// <returns>Task with Session</returns>
        public Task<Session> GetCurrentSessionAsync()
        {
            return _sessions.GetCurrentSessionAsync();
        }

        #endregion
    }
}
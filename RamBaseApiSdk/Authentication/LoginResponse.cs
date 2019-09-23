using System;

namespace RamBase.Api.Sdk.Authentication
{
    public class LoginResponse
    {
        public string AccessToken { get; internal set; }
        public string RefreshToken { get; internal set; }
        public DateTime ExpireTime { get; internal set; }
        public bool IsTargetTestSystem { get; internal set; }

        public LoginResponse(string accessToken, string refreshToken, DateTime expireTime, bool isTargetTestSystem)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpireTime = expireTime;
            IsTargetTestSystem = isTargetTestSystem;
        }

    }
}

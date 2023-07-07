using System;

namespace RamBase.Api.Sdk.Authentication
{
    public class LoginResponse
    {
        public string AccessToken { get; }
        public string RefreshToken { get; }
        public DateTime ExpireTime { get; }
        public bool IsTargetTestSystem { get; }

        public LoginResponse(string accessToken, string refreshToken, DateTime expireTime, bool isTargetTestSystem)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpireTime = expireTime;
            IsTargetTestSystem = isTargetTestSystem;
        }

    }
}

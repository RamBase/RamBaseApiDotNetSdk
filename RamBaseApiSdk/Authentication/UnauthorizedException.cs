using System;

namespace RamBaseApiSdk.Authentication
{
    /// <summary>
    /// Exception is thrown if RamBase API returns a status 401 - Unauthorized
    /// </summary>
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("HTTP Status 401 - Unauthorized") { }
    }
}

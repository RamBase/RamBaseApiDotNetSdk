using System;
using System.Collections.Generic;

namespace RamBase.Api.Sdk.Authentication
{
    public class OtpRequiredException : Exception 
    {
        public List<Target> Targets { get; private set; }
        public OtpRequiredException(List<Target> targets) :base("Otp Required")
        {
            Targets = targets;
        }
    }
}

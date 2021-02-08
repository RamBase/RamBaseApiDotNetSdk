using System;
using System.Collections.Generic;

namespace RamBase.Api.Sdk.Authentication
{
    public class OtpRequiredException : Exception
    {
        public List<Target> Targets { get; private set; }

        public OTPDeliveryMethod? OTPDeliveryMethod { get; set; }

        public OtpRequiredException(List<Target> targets, OTPDeliveryMethod? deliveryMethod) : base("Otp Required")
        {
            Targets = targets;
            OTPDeliveryMethod = deliveryMethod;
        }
    }

    public enum OTPDeliveryMethod
    {
        SMS,
        EMAIL,
        TOTP
    }
}

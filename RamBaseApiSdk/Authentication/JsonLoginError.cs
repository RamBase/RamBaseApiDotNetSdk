using System.Collections.Generic;

namespace RamBase.Api.Sdk.Authentication
{
    internal class JsonLoginError
    {
        public int error_code { get; set; }
        public string message { get; set; }
        public bool otp_required { get; set; } = false;
        public OTPDeliveryMethod? otp_method { get; set; }

        public List<Target> targets { get; set; }
    }
}

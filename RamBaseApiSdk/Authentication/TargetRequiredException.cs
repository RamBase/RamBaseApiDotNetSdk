using System;
using System.Collections.Generic;
using System.Text;

namespace RamBase.Api.Sdk.Authentication
{
    public class TargetRequiredException : Exception
    {
        public List<Target> Targets { get; private set; }
        public TargetRequiredException(List<Target> targets) : base("Target is required")
        {
            Targets = targets;
        }
    }
}

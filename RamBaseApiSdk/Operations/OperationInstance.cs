using RamBase.Api.Sdk.Request;
using System;

namespace RamBase.Api.Sdk.Operations
{
    public class OperationInstance
    {
        public int OperationInstanceId { get; set; }
        public OperationStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public CreatedBy CreatedBy { get; set; }
        public CreatedFor CreatedFor { get; set; }
        public ApiOperation ApiOperation { get; set; }
        public ObjectReferance ObjectReference { get; set; }
        public Error Error { get; set; }
        public SystemJob SystemJob { get; set; }
        public string OperationInstanceLink { get; set; }
    }
}

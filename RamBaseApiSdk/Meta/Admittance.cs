namespace RamBase.Api.Sdk.Meta
{
    public class Admittance
    {
		/// <summary>
		/// Reason phrase for why access is denied to this resource. Null if access is permitted.
		/// </summary>
		public string AccessDenied { get; set; }

		/// <summary>
		/// Id of access rule that denied access to the resource. Null if access is permittied or access was denied by a missing permission.
		/// </summary>
		public int? AccessRuleId { get; set; }

		/// <summary>
		/// Id of permission that denied access to the resource. Null if access is permitted or access was denied by an access rule.
		/// </summary>
		public int? MissingPermissionId { get; set; }

		/// <summary>
		/// Id of the error message explaining why access was denied. Null if access is permitted.
		/// </summary>
		public int? ErrorMessageId { get; set; }
	}
}

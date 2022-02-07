using System;
using System.Collections.Generic;
using System.Text;

namespace RamBase.Api.Sdk.Meta
{
	public class InputField
	{
		/// <summary>
		/// Name of the input field
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Translated name of the input field. Language determined by the $lang parameter
		/// </summary>
		public string TranslatedName { get; set; }

		/// <summary>
		/// Datatype of the input field.
		/// DateTime, Date, Time, Integer, Long, Decimal, Boolean or String
		/// </summary>
		public string Datatype { get; set; }

		/// <summary>
		/// JSON path of the input field
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// Description of the input field
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Translated description of the input field. Language is determined by the $lang parameter
		/// </summary>
		public string TranslatedDescription { get; set; }

		/// <summary>
		/// True if this field is required as input to the request
		/// </summary>
		public bool IsRequired { get; set; }

		/// <summary>
		/// Minimum value of the input field. Null if no minimum.
		/// </summary>
		public int? MinimumValue { get; set; }

		/// <summary>
		/// Maximum value of the input field. Null if no maximum.
		/// </summary>
		public int? MaximumValue { get; set; }

		/// <summary>
		/// Default value of the input field. Null if no default.
		/// </summary>
		public string DefaultValue { get; set; }

		/// <summary>
		/// Regular expression restriction of the input field. Null if no regular expression restriction.
		/// </summary>
		public string RegularExpression { get; set; }

		/// <summary>
		/// Applicable domain values of the input field. Null if no applicable domain values.
		/// </summary>
		public string ApplicableDomainValues { get; set; }

		/// <summary>
		/// Reason phrase for why access is denied to this field. Null if access is permitted.
		/// </summary>
		public string AccessDenied { get; set; }

		/// <summary>
		/// Id of access rule that denied access to the field. Null if access is permittied or access was denied by a missing permission.
		/// </summary>
		public int? AccessRuleId { get; set; }

		/// <summary>
		/// Id of permission that denied access to the field. Null if access is permitted or access was denied by an access rule.
		/// </summary>
		public int? MissingPermissionId { get; set; }

		/// <summary>
		/// Id of the error message explaining why access was denied. Null if access is permitted.
		/// </summary>
		public int? ErrorMessageId { get; set; }
	}
}

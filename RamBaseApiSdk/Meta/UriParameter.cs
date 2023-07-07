namespace RamBase.Api.Sdk.Meta
{
	public class UriParameter
	{
		/// <summary>
		/// Name/Key of the URI parameter.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Datatype of the URI parameter.
		/// </summary>
		public string Datatype { get; set; }

		/// <summary>
		/// Description of the URI parameter.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Translated description of the URI parameter. Language determined by $land parameter.
		/// </summary>
		public string TranslatedDescription { get; set; }

		/// <summary>
		/// True if this parameter is required when calling the resource.
		/// </summary>
		public bool IsRequired { get; set; }

		/// <summary>
		/// Minimum value of the URI parameter. Null if no minimum.
		/// </summary>
		public int? MinimumValue { get; set; }

		/// <summary>
		/// Maximum value of the URI parameter. Null if no maximum.
		/// </summary>
		public int? MaximumValue { get; set; }

		/// <summary>
		/// Default value of the URI parameter. Null if no default.
		/// </summary>
		public string DefaultValue { get; set; }

		/// <summary>
		/// Regular expression restriction of the URI parameter. Null if no regular expression restriction.
		/// </summary>
		public string RegularExpression { get; set; }

		/// <summary>
		/// Applicable domain values of the URI parameter. Null if no applicable domain values.
		/// </summary>
		public string ApplicableDomainValues { get; set; }
	}
}

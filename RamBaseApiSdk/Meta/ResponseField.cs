namespace RamBase.Api.Sdk.Meta
{
	public class ResponseField
	{
		/// <summary>
		/// Name of the response field.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Translated name of the response filed. Language is determined by the $lang parameter.
		/// </summary>
		public string TranslatedName { get; set; }

		/// <summary>
		/// Datatype of the response field.
		/// DateTime, Date, Time, Integer, Long, Decimal, Boolean or String.
		/// </summary>
		public string Datatype { get; set; }

		/// <summary>
		/// JSON path of the response field.
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// Description of the response field.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Translated description of the response field. Langugage is determined by the $lang parameter.
		/// </summary>
		public string TranslatedDescription { get; set; }

		/// <summary>
		/// True if this field has a domain description. Domain descriptions can be requested with the $showDomainDescriptions parameter
		/// </summary>
		public bool HasDomainDescription { get; set; }

		/// <summary>
		/// Applicable domain values of the response field. Null if no applicable domain values.
		/// </summary>
		public string ApplicableDomainValues { get; set; }

		/// <summary>
		/// If this response field is not returned by default this property will contain the parameter to use with $expand to request it.
		/// </summary>
		public string ExpandParameter { get; set; }
	}
}

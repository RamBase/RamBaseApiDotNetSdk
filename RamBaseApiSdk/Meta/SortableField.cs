namespace RamBase.Api.Sdk.Meta
{
    public class SortableField
    {
		/// <summary>
		/// Name of the sortable field
		/// </summary>
        public string Name { get; set; }

		/// <summary>
		/// Translated name of the sortable field. Language determined by $lang parameter
		/// </summary>
		public string TranslatedName { get; set; }

		/// <summary>
		/// Datatype of the sortable field
		/// </summary>
		public string Datatype { get; set; }

		/// <summary>
		/// Description of the sortable field
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Translated description of the sortable field. Language determined by $lang parameter
		/// </summary>
		public string TranslatedDescription { get; set; }
    }
}
using RamBaseApiSdk.Meta;
using System.Collections.Generic;

namespace RamBase.Api.Sdk.Meta
{
    public class NamedFilter
    {
        /// <summary>
        /// Name of the filter
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Translated name of the filter. Language determined by $lang parameter
        /// </summary>
		public string TranslatedName { get; set; }

        /// <summary>
        /// Key used for filtering with this filter
        /// </summary>
		public string Key { get; set; }

        /// <summary>
        /// Description of the filter
        /// </summary>
		public string Description { get; set; }


        /// <summary>
        /// Translated description of the filter. Language determined by $lang parameter
        /// </summary>
		public string TranslatedDescription { get; set; }

        /// <summary>
        /// List of filters this filter can not be combined with
        /// </summary>
        public List<UncombinableFilter> UncombinableWith { get; set; }
    }
}

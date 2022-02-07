using System;
using System.Collections.Generic;
using System.Text;

namespace RamBase.Api.Sdk.Meta
{
    public class FilterField
    {
        /// <summary>
        /// Name of the filter field
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Translated name of the filter field. Language determined by $lang parameter
        /// </summary>
        public string TranslatedName { get; set; }
        
        /// <summary>
        /// Datatype of the filter field
        /// </summary>
        public string Datatype { get; set; }
        
        /// <summary>
        /// Description of the input field
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Translated description of the filter field. Language determined by $lang parameter
        /// </summary>
        public string TranslatedDescription { get; set; }
		
        /// <summary>
        /// Applicable domain values of the filter field. Null if no applicable domain valyes
        /// </summary>
        public string ApplicableDomainValues { get; set; }

        /// <summary>
        /// Macro set this field belongs to. Null if none. The macro set determines which macros can be used when filtering on the field.
        /// </summary>
		public string MacroSet { get; set; }
	}
}

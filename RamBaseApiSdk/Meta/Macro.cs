using System;
using System.Collections.Generic;
using System.Text;

namespace RamBase.Api.Sdk.Meta
{
    public class Macro
    {
        /// <summary>
        /// Name of the macro
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Comma separated list of sets this macro belongs to
        /// </summary>
		public string MacroSets { get; set; }

        /// <summary>
        /// Comma separated list of units that can be used with this macro. Can be null.
        /// </summary>
        public string AllowedUnits { get; set; }

        /// <summary>
        /// Default unit. Can be null.
        /// </summary>
        public string DefaultUnit { get; set; }
	}
}

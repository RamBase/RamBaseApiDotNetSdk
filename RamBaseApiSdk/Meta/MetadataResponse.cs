using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace RamBase.Api.Sdk.Meta
{
    public class MetadataResponse
    {
        /// <summary>
        /// Description of the resource
        /// </summary>
		public string Description { get; set; }

        /// <summary>
        /// URI of the resource
        /// </summary>
		public string Uri { get; set; }

        /// <summary>
        /// Maximum number of row that may be requested using the $top parameter. This only applies to list resources.
        /// </summary>
		public int? MaximumResultRows { get; set; }

        /// <summary>
        /// Resource level admittance restriction. This is null if access to the resources is permitted.
        /// </summary>
		public Admittance Admittance { get; set; }

        /// <summary>
        /// URI parameters for the resources. This includes both path and query parameters.
        /// </summary>
		public List<UriParameter> UriParameters { get; set; }

        /// <summary>
        /// List of fields that may be sent as input to this resource. Only applies to POST and PUT.
        /// </summary>
		public List<InputField> InputFields { get; set; }

        /// <summary>
        /// List of fields that is returned from this resource.
        /// </summary>
        public List<ResponseField> ResponseFields { get; set; }

        /// <summary>
        /// List of fields that can be filtered with the $filter parameter. Only applies to list resources.
        /// </summary>
        public List<FilterField> FilterFields { get; set; }

        /// <summary>
        /// List of fields than can be used for sorting with the $orderby parameter. Only applies to list resources.
        /// </summary>
        public List<SortableField> SortableFields { get; set; }

        /// <summary>
        /// List of named filters that can be used with the $filter parameter. Only applies to list resources.
        /// </summary>
        public List<NamedFilter> NamedFilters { get; set; }
    }
}

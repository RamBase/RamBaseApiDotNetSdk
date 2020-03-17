using System;
using System.Collections.Generic;
using System.Text;

namespace RamBase.Api.Sdk.Request
{
    public class GetParameters : QueryParameters
    {
        /// <summary>
        /// Only usable when the result is a list. Set the number of objects to return in result list. Default is 10. Maximum limit is specific for each resource, but is default 80.
        /// </summary>
        public int? Top { get; set; }

        /// <summary>
        /// Only usable when the result is a list. Set the number of objects to return in result list to maximum value. Default is 10. Overwrites Top.
        /// </summary>
        public bool TopMax { get; set; } = false;

        /// <summary>
        /// Use to show an additional descriptional field for fields that are using Domain Values.
        /// </summary>
        public bool ShowDomainDescriptions { get; set; } = false;

        /// <summary>
        /// Only usable when the result is a list. Use to filter the result list, see the Filter and OrderBy page for more information about this
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Only usable when the result is a list. Set to "allpages" if you want size of the result list to be returned. Accepted values are: "allpages", "none"
        /// </summary>
        public string InlineCount { get; set; }

        /// <summary>
        /// Only usable when the result is a list. Set the number of objects to skip in the result list.
        /// </summary>
        public string Skip { get; set; }

        /// <summary>
        /// Only usable when the result is a list. Use to order the result list, see Filter and OrderBy page for more information about this.
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// Use to define the fields to return in the result. Accepts a comma-separated list of Output Field names
        /// </summary>
        public string Select { get; set; }

        /// <summary>
        /// Use to expand Output fields that are not shown by default in the output result. Possible Expandable fields are listed in the Output tab for the resource. Accepts a comma-separated list of expandable Output Field names
        /// </summary>
        public string Expand { get; set; }

        /// <summary>
        /// Creates querystring from set parameters
        /// </summary>
        /// <returns>Querystring</returns>
        public override string ToString()
        {
            string value = base.ToString();

            if (!value.EndsWith("?"))
                value += "&";

            if (TopMax)
                value += $"$top=$max&";
            else if (Top.HasValue)
                value += $"$top={Top}&";

            if (ShowDomainDescriptions)
                value += $"$showDomainDescriptions=true&";

            if (!string.IsNullOrEmpty(Filter))
                value += $"$filter={Filter}&";

            if (!string.IsNullOrEmpty(InlineCount))
                value += $"$inlinecount={InlineCount}&";

            if (!string.IsNullOrEmpty(Skip))
                value += $"$skip={Skip}&";

            if (!string.IsNullOrEmpty(OrderBy))
                value += $"$orderby={OrderBy}&";

            if (!string.IsNullOrEmpty(Select))
                value += $"$select={Select}&";

            if (!string.IsNullOrEmpty(Expand))
                value += $"$expand={Expand}&";

            return value.TrimEnd('&');
        }
    }
}

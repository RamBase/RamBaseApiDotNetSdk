using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace RamBase.Api.Sdk.Meta
{
    public class Metadata
    {
        public Admittance Admittance { get; set; }
        public List<Field> Fields { get; set; }
        public List<FilterField> FilterFields { get; set; }
        public List<SortableField> SortableFields { get; set; }
        public List<NamedFilter> NamedFilters { get; set; }

        internal Metadata(MetadataResponse metadataResponse, ApiResourceVerb verb)
        {
            this.Admittance = metadataResponse.Admittance;
            this.Fields = GetFields(metadataResponse.Fields, verb);
            this.FilterFields = GetFilterFields(metadataResponse.FilterFields);
            this.SortableFields = GetSortableFields(metadataResponse.SortableFields);
            this.NamedFilters = metadataResponse.NamedFilters;
        }

        /// <summary>
        /// Convert fields from dynamic to static objects
        /// </summary>
        /// <param name="fields">Metadata fields</param>
        /// <param name="verb">Http verb</param>
        /// <returns>List of fields</returns>
        private List<Field> GetFields(dynamic fields, ApiResourceVerb verb)
        {
            List<Field> metadataFields = new List<Field>();
            if (verb == ApiResourceVerb.PUT)
                foreach (KeyValuePair<string, dynamic> field in fields)
                {
                    FieldPut put = new FieldPut(field);
                    metadataFields.Add(put);
                }
            else if (verb == ApiResourceVerb.GET)
                foreach (KeyValuePair<string, dynamic> field in fields)
                {
                    FieldGet get = new FieldGet(field);
                    metadataFields.Add(get);
                }
            else if (verb == ApiResourceVerb.POST)
                foreach (KeyValuePair<string, dynamic> field in fields)
                {
                    FieldPost post = new FieldPost(field);
                    metadataFields.Add(post);
                }

            return metadataFields;
        }

        private List<FilterField> GetFilterFields(dynamic filterFieldsDynamic)
        {
            List<FilterField> filterFields = new List<FilterField>();

            foreach (KeyValuePair<string, dynamic> filterField in filterFieldsDynamic)
                filterFields.Add(new FilterField(filterField));

            return filterFields;
        }

        private List<SortableField> GetSortableFields(dynamic sortableFieldsDynamic)
        {
            List<SortableField> sortableFields = new List<SortableField>();

            foreach (KeyValuePair<string, dynamic> sortableField in sortableFieldsDynamic)
                sortableFields.Add(new SortableField(sortableField));

            return sortableFields;
        }

        /// <summary>
        /// Finds field with matching xPath
        /// </summary>
        /// <param name="xPath">XPath to match</param>
        /// <returns>First field with matching xPath or null</returns>
        public Field FindField(string xPath)
        {
            foreach (Field field in Fields)
                if (field.XPath == xPath)
                    return field;
            return null;
        }

        /// <summary>
        /// Finds all fields with matching or partially matching xPath
        /// </summary>
        /// <param name="xPath">XPath to match</param>
        /// <returns>List of matching fields</returns>
        public List<Field> FindFields(string xPath)
        {
            return Fields.Where(f => f.XPath.Contains(xPath)).ToList();
        }

        /// <summary>
        /// Finds field with matching name
        /// </summary>
        /// <param name="name">Name to match</param>
        /// <returns>First field with matching name or null</returns>
        public Field FindFieldByName(string name)
        {
            foreach (Field field in Fields)
                if (field.Name.ToLower() == name.ToLower())
                    return field;
            return null;
        }

        /// <summary>
        /// Finds filter field with matching name
        /// </summary>
        /// <param name="name">Name to match</param>
        /// <returns>First filter field with matching name</returns>
        public FilterField FindFilterFieldByName(string name)
        {
            foreach (FilterField field in FilterFields)
                if (field.Name.ToLower() == name.ToLower())
                    return field;
            return null;
        }

        /// <summary>
        /// Finds sortable field with matching name
        /// </summary>
        /// <param name="name">Name to match</param>
        /// <returns>First sortable field with matching name or null</returns>
        public SortableField FindSortableFieldbyName(string name)
        {
            foreach (SortableField sortableField in SortableFields)
                if (sortableField.Name.ToLower() == name.ToLower())
                    return sortableField;
            return null;
        }

        /// <summary>
        /// Finds named filter with matching name
        /// </summary>
        /// <param name="name">Name to match</param>
        /// <returns>First named filter with matching name or null</returns>
        public NamedFilter FindNamedFilterByName(string name)
        {
            foreach (NamedFilter namedFilter in NamedFilters)
                if (namedFilter.FilterName.ToLower() == name.ToLower())
                    return namedFilter;
            return null;
        }
    }
}

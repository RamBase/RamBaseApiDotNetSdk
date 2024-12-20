using System.Collections.Generic;
using System.Text;

namespace RamBase.Api.Sdk.Request
{
	public abstract class QueryParameters
	{

		/// <summary>
		/// Set the database to use for the request
		/// </summary>
		public string Db { get; set; }

		/// <summary>
		/// Use to get localized responses. 
		/// </summary>
		public string Lang { get; set; }

		/// <summary>
		/// This can be used to test a new version of a resource when your ApiClient is running an older, deprecated, version of a resource. UseMinimumVersion can be used to test against a newer version of the resource
		/// </summary>
		public int? UseMinimumVersion { get; set; }

		/// <summary>
		/// Additional query parameters
		/// </summary>
		public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();

		/// <summary>
		/// Add new parameter
		/// </summary>
		/// <param name="parameter">Parameter name</param>
		/// <param name="value">Parameter value</param>
		public void Add(string parameter, string value)
		{
			Parameters.Add(parameter, value);
		}

		/// <summary>
		/// Creates querystring from set parameters
		/// </summary>
		/// <returns>Querystring</returns>
		public string Build()
		{
			return ToString();
		}

		/// <summary>
		/// Creates querystring from set parameters
		/// </summary>
		/// <returns>Querystring</returns>
		public override string ToString()
		{
			var value = new StringBuilder();

			if (!string.IsNullOrEmpty(Db))
				value.Append($"$db={Db}&");

			if (!string.IsNullOrEmpty(Lang))
				value.Append($"$lang={Lang}&");

			if (UseMinimumVersion.HasValue)
				value.Append($"$useMinimumVersion={UseMinimumVersion}&");

			foreach (var parameter in Parameters)
				value.Append($"{parameter.Key}={parameter.Value}&");

			if (value.Length > 0)
				value.Remove(value.Length - 1, 1);

			return value.ToString();
		}
	}
}

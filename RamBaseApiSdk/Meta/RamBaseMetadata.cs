using RamBase.Api.Sdk.Request;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace RamBase.Api.Sdk.Meta
{
	internal class RamBaseMetadata
	{
		private readonly RamBaseRequest _request;

		public RamBaseMetadata(RamBaseRequest request)
		{
			_request = request;
		}

		/// <summary>
		/// Asynchronously gets metadata for given resource
		/// </summary>
		/// <param name="uri">Relative or explicit path to a resource/endpoint</param>
		/// <param name="verb">Metadata for given verb</param>
		/// <param name="headers">Request headers</param>
		/// <returns>Task with metadata</returns>
		/// <exception cref="RequestException">HTTP status not successful</exception>
		public async Task<MetadataResponse> GetMetadataAsync(string uri, ApiResourceVerb verb, Headers headers = null)
		{
			var parameters = $"?uri={uri}&verb={verb}";
			parameters += "&$expand=ApplicableDomainValues,ExpandableFields,Description,TranslatedDescription,TranslatedName,MacroDefinitions,URIParameters";
			const string url = "metadata";
			var response = await _request.PerformRequestAsync(ApiResourceVerb.GET, url, parameters: parameters, headers: headers);
			var metadataResponse = JsonConvert.DeserializeObject<MetadataWrapper>(response.Content);
			return metadataResponse.Metadata;
		}

		private class MetadataWrapper
		{
			public MetadataResponse Metadata { get; set; }
		}
	}
}

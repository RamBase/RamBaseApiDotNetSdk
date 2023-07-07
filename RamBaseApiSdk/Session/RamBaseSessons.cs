using RamBase.Api.Sdk.Request;
using System.Threading.Tasks;

namespace RamBase.Api.Sdk.Sessions
{
    internal class RamBaseSessions
    {
        private readonly RamBaseRequest _request;

        public RamBaseSessions(RamBaseRequest request)
        {
            _request = request;
        }
        /// <summary>
        /// Asynchronously gets information about the current session
        /// </summary>
        /// <param name="headers">Request headers</param>
        /// <returns>Task with Session</returns>
        public async Task<Session> GetCurrentSessionAsync(Headers headers = null)
        {
            var response = await _request.PerformRequestAsync(ApiResourceVerb.GET, "system/sessions/current", headers: headers);
            return response.To<SessionWrapper>().Session;
        }
    }
}

using RamBase.Api.Sdk.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RamBase.Api.Sdk.Sessions
{
    internal class RamBaseSessions
    {
        private RamBaseRequest _request;

        public RamBaseSessions(RamBaseRequest request)
        {
            _request = request;
        }
        /// <summary>
        /// Asynchronously gets information about the current session
        /// </summary>
        /// <returns>Task with Session</returns>
        public async Task<Session> GetCurrentSessionAsync()
        {
            ApiResponse response = await _request.PerformRequestAsync(ApiResourceVerb.GET, "system/sessions/current");
            return response.To<SessionWrapper>().Session;
        }
    }
}

using System.Collections.Generic;

namespace RamBase.Api.Sdk.Request
{
    public class Headers
    {
        public void ImpersonateUser(string user)
        {
            RequestHeaders.Add("Impersonate-User", user);
        }
        
       public Dictionary<string, string> RequestHeaders { get; set; } = new Dictionary<string, string>();

       public void Add(string name, string value)
       {
           RequestHeaders.Add(name, value);
       }
       
    }
}
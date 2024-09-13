using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace WholeSaler.Web.Utility
{
    public class SetAuthHeader
    {
   

        public SetAuthHeader()
        {
       
        }
        public static bool SetAuthorizationHeader(HttpRequest request, HttpClient httpClient)
        {
     
            var accessToken = request.Cookies["AccessToken"];
            if (!string.IsNullOrEmpty(accessToken))
            {
 
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                return true;
            }
            return false;

        }
    }
}

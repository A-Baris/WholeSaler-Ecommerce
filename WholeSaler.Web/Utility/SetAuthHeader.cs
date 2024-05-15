using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace WholeSaler.Web.Utility
{
    public class SetAuthHeader
    {
        public static bool SetAuthorizationHeader(HttpClient _httpClient, HttpRequest request)
        {
            var accessToken = request.Cookies["AccessToken"];
            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                return true;
            }
            return false;

        }
    }
}

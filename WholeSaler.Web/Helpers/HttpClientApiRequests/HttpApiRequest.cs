using System.Net.Http;

namespace WholeSaler.Web.Helpers.HttpClientApiRequests
{
    public class HttpApiRequest
    {

        private readonly HttpClient _httpClient;

        public HttpApiRequest(HttpClient httpClient)
        {
            _httpClient = httpClient;
            
        }
        public  async Task<HttpResponseMessage> PostAsync<T>(string apiUri, T content)
        {
           
            return await _httpClient.PostAsJsonAsync(apiUri, content);
        }

      
        public  async Task<HttpResponseMessage> PutAsync<T>(string apiUri, T content)
        {
            return await _httpClient.PutAsJsonAsync(apiUri, content);
        }

   
        public  async Task<T> GetAsync<T>(string apiUri)
        {
            return await _httpClient.GetFromJsonAsync<T>(apiUri);
        }
    }

}

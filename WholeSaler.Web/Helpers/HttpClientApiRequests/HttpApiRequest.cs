using Microsoft.AspNet.SignalR.Client.Http;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.Net.Http;
using WholeSaler.Web.Models.ViewModels.ShoppingCartVM;

namespace WholeSaler.Web.Helpers.HttpClientApiRequests
{
    public class HttpApiRequest : IHttpApiRequest
    {

        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory httpClientFactory;

        public HttpApiRequest(IHttpClientFactory httpClientFactory)
        {

            this.httpClientFactory = httpClientFactory;
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<HttpResponseMessage> PostAsync<T>(string apiUri, T model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                return await _httpClient.PostAsync(apiUri, content);
            }
            catch (Exception ex)
            {

                throw new HttpRequestException($"Error occurred while making POST request to {apiUri}.", ex);
            }
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string apiUri, T model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                return await _httpClient.PutAsync(apiUri, content);
            }
            catch (Exception ex)
            {

                throw new HttpRequestException($"Error occurred while making PUT request to {apiUri}.", ex);
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string apiUri)
        {
            try
            {
                return await _httpClient.GetAsync(apiUri);
            }
            catch (Exception ex)
            {

                throw new HttpRequestException($"Error occurred while making GET request to {apiUri}.", ex);
            }
        }
        public async Task<HttpResponseMessage>DeleteAsync(string apiUri)
        {
            try
            {
                return await _httpClient.DeleteAsync(apiUri);
            }
            catch (Exception ex)
            {

                throw new HttpRequestException($"Error occurred while making Delete request to {apiUri}.", ex);
            }
        }


            public async Task<List<T>> DeserializeJsonToModelForList<T>(HttpResponseMessage response)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<T>>(jsonData);
                return data;
            }

        public async Task<T> DeserializeJsonToModelForSingle<T>(HttpResponseMessage response)
        {
            var jsonData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<T>(jsonData);
            return data;
        }
    }


}

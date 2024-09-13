using WholeSaler.Web.Models.ViewModels.Product.Custom;

namespace WholeSaler.Web.Helpers.HttpClientApiRequests
{
    public interface IHttpApiRequest
    {
        Task<HttpResponseMessage> PostAsync<T>(string apiUri, T model);
        Task<HttpResponseMessage> PutAsync<T>(string apiUri, T model);
        Task<HttpResponseMessage> GetAsync(string apiUri);
        Task<HttpResponseMessage> DeleteAsync(string apiUri);
        Task<List<T>> DeserializeJsonToModelForList<T>(HttpResponseMessage response);
        Task<T> DeserializeJsonToModelForSingle<T>(HttpResponseMessage response);
    }
}

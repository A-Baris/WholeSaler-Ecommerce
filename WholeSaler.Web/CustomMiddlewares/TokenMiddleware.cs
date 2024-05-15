using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using WholeSaler.Web.Models.ViewModels.UserVM;

namespace WholeSaler.Web.CustomMiddleWares
{
    public class TokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        public TokenMiddleware(RequestDelegate next, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _next = next;
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();

        }

        public async Task Invoke(HttpContext context)
        {
            // Kullanıcıya ait bir accessToken var mı kontrol et
            var accessToken = context.Request.Cookies["AccessToken"];

            if (string.IsNullOrEmpty(accessToken))
            {
                // Kullanıcıya ait bir refreshToken var mı kontrol et
                var refreshToken = context.Request.Cookies["RefreshToken"];
                if (refreshToken == null)
                {
                     await _next(context);
                    return;
                }

                if (!string.IsNullOrEmpty(refreshToken))
                {
                    // refreshToken'ı kullanarak yeni bir accessToken al
                    var newToken =  await RefreshAccessTokenAsync(refreshToken);

                    if (newToken != null)
                    {
                        // Yeni accessToken ve refreshToken'ı kullanıcıya cookie olarak atama
                        context.Response.Cookies.Append("AccessToken", newToken.AccessToken, new CookieOptions
                        {
                            HttpOnly = true,
                            SameSite = SameSiteMode.None,
                            Secure = true // Uyarı: HTTPS kullanıyorsanız true olarak ayarlayın
                        });

                        context.Response.Cookies.Append("RefreshToken", newToken.RefreshToken, new CookieOptions
                        {
                            HttpOnly = true,
                            SameSite = SameSiteMode.None,
                            Secure = true // Uyarı: HTTPS kullanıyorsanız true olarak ayarlayın
                        });
                      
                    }
                }
            }
           
               
                  
                

                // Proceed to the next middleware
                 await _next(context);
            
        }
        private async Task<TokenVM> RefreshAccessTokenAsync(string refreshToken)
        {

            var apiUrl = $"https://localhost:7185/api/user/refresh-token/{refreshToken}";       
            var response =  await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var tokenJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TokenVM>(tokenJson);
            }

            return null;
        }
    }
}

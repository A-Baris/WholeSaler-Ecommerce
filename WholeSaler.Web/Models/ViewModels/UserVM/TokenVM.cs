namespace WholeSaler.Web.Models.ViewModels.UserVM
{
    public class TokenVM
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
        public string? ErrorMessage { get; set; }
    }
}

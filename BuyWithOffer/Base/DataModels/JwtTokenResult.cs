namespace BuyWithOffer
{
    public class JwtTokenResult
    {
        public string AccessToken { get; set; }
        public int ExpireInSeconds { get; set; }
        public string UserId { get; set; }
    }
}

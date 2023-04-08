namespace BLL
{
    public class JwtSettings
    {
        public string? Issuer { get; set; }
        public string? Key { get; set; }
        public double Lifetime { get; set; }
        public string? Audience { get; set; }
    }
}

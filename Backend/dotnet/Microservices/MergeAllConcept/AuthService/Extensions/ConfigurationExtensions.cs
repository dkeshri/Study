namespace AuthService.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetTokenSecret(this IConfiguration configuration)
        {
            return configuration.GetSection("Jwt:Token:Secret").Value!;
        }
        public static int GetTokenExpireInSec(this IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));
            var tokenExpireInSecString = configuration.GetSection("Jwt:Token:ExpireInSec").Value;

            return int.TryParse(tokenExpireInSecString, out var tokenExpiresInSeconds) ? tokenExpiresInSeconds : 86400;
        }
    }
}

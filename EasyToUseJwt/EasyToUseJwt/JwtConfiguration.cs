namespace EasyToUseJwt
{
    public sealed class JwtConfiguration
    {
        public JwtConfiguration()
        {
        }

        public JwtConfiguration(string secretKey, bool saveToken = true, bool requireHttpsMetadata = true)
        {
            SecretKey = secretKey;
            RequireHttpsMetadata = requireHttpsMetadata;
            SaveToken = saveToken;
        }

        public string SecretKey { get; set; }
        public bool SaveToken { get; set; }
        public bool RequireHttpsMetadata { get; set; }
    }
}

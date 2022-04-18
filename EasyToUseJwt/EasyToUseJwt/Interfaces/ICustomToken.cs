using System.Security.Claims;

namespace EasyToUseJwt.Interfaces
{
    public interface ICustomToken
    {
        public string Generate(Claim[] claims, string secretKey, DateTime? expiresIn = null);
        public Task<string> GenerateAsync(Claim[] claims, string secretKey, DateTime? expiresIn = null);
    }
}

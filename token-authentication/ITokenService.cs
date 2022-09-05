using System.Security.Claims;

namespace token_authentication
{
    public interface ITokenService
    {
        string GenereteAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}

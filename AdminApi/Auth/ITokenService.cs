using System.Security.Claims;

namespace AdminApi.Auth;

public interface ITokenService
{
    string CreateToken(IEnumerable<Claim> claims);
}
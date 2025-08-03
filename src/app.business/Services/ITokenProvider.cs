using System.Security.Claims;

namespace app.business.Services;
public interface ITokenProvider
{
    string Generate(IEnumerable<Claim> claims);
}

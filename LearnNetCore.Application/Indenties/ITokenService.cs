using LearnNetCore.Application.Identities;

namespace LearnNetCore.Application.Indenties;

public interface ITokenService
{
    string CreateToken(ApplicationUser user);

}

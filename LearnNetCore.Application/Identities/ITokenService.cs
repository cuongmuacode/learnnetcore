namespace LearnNetCore.Application.Identities;

public interface ITokenService
{
    string CreateToken(ApplicationUser user);

}

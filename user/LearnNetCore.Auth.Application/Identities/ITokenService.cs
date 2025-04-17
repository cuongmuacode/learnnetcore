namespace LearnNetCore.Auth.Application.Identities;

public interface ITokenService
{
    string CreateToken(ApplicationUser user);

}

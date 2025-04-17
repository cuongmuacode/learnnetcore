namespace LearnNetCore.Auth.Application.Models;

public class UserResponseModel
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Token { get; set; }
}

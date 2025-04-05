using LearnNetCore.Application.Identities;
using LearnNetCore.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearnNetCore.Api.Controllers;

/// <summary>
/// 
/// </summary>
[Route("api/accounts")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="tokenService"></param>
    /// <param name="signInManager"></param>
    public AccountController(UserManager<ApplicationUser> userManager, ITokenService tokenService, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(UserResponseModel), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestModel registerRequestModel)
    {
        var user = new ApplicationUser
        {
            UserName = registerRequestModel.UserName,
            Email = registerRequestModel.Email
        };
        var result = await _userManager.CreateAsync(user, registerRequestModel.Password);
        if (result.Succeeded)
        {
            return Ok(new UserResponseModel()
            {
                UserName = user.UserName,
                Email = user.Email
            });
        }
        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(UserResponseModel), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserModel loginUserModel)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.UserName == loginUserModel.UserName.ToLower());

        if (user == null)
        {
            return Unauthorized("Invalid username!");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginUserModel.Password, false);
        if (!result.Succeeded) return Unauthorized("User not found and/or password incorrect");
        return Ok(new UserResponseModel()
        {
            UserName = user.UserName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Id = user.Id,
            Token = _tokenService.CreateToken(user)
        });
    }
}

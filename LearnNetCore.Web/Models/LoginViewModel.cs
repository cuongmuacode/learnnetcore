using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LearnNetCore.Web.Models;

public class LoginViewModel
{
    [Required]
    [DisplayName("User name")]
    public string UserName { get; set; }

    [DisplayName("Password")]
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}

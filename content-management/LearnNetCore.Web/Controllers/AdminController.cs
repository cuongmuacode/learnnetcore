using LearnNetCore.Application.Identities;
using LearnNetCore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearnNetCore.Web.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // Danh sách user
    public IActionResult Index()
    {
        var users = _userManager.Users.ToList();
        return View(users);
    }

    // GET: Phân quyền cho user
    public async Task<IActionResult> ManageRoles(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        var model = new EditUserRolesViewModel
        {
            UserId = user.Id,
            UserName = user.UserName,
            Roles = new List<RoleCheckbox>()
        };

        foreach (var role in _roleManager.Roles)
        {
            model.Roles.Add(new RoleCheckbox
            {
                RoleName = role.Name,
                IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
            });
        }

        return View(model);
    }

    // POST: Cập nhật quyền
    [HttpPost]
    public async Task<IActionResult> ManageRoles(EditUserRolesViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null) return NotFound();

        var roles = await _userManager.GetRolesAsync(user);
        var result = await _userManager.RemoveFromRolesAsync(user, roles);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Không thể xoá quyền cũ");
            return View(model);
        }
        result = await _userManager.AddToRolesAsync(
            user,
            model?.Roles?
                .Where(r => r.IsSelected)?
                .Select(r => r.RoleName) ?? new string[] { }
        );

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Không thể thêm quyền mới");
            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }
}

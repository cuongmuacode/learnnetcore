using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LearnNetCore.Web.Controllers;

[Authorize(Roles = "Admin")]
public class RoleController : Controller
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleController(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    // GET: Danh sách Role
    public IActionResult Index()
    {
        var roles = _roleManager.Roles.ToList();
        return View(roles);
    }

    // GET: Tạo Role
    public IActionResult Create()
    {
        return View();
    }

    // POST: Tạo Role
    [HttpPost]
    public async Task<IActionResult> Create(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
        {
            ModelState.AddModelError("", "Tên role không được để trống");
            return View();
        }

        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (roleExists)
        {
            ModelState.AddModelError("", "Role đã tồn tại");
            return View();
        }

        await _roleManager.CreateAsync(new IdentityRole(roleName));
        return RedirectToAction(nameof(Index));
    }

    // GET: Sửa Role
    public async Task<IActionResult> Edit(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null) return NotFound();
        return View(role);
    }

    // POST: Sửa Role
    [HttpPost]
    public async Task<IActionResult> Edit(string id, string roleName)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null) return NotFound();

        if (string.IsNullOrWhiteSpace(roleName))
        {
            ModelState.AddModelError("", "Tên role không được để trống");
            return View(role);
        }

        role.Name = roleName;
        await _roleManager.UpdateAsync(role);
        return RedirectToAction(nameof(Index));
    }

    // POST: Xoá Role
    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null) return NotFound();

        await _roleManager.DeleteAsync(role);
        return RedirectToAction(nameof(Index));
    }
}

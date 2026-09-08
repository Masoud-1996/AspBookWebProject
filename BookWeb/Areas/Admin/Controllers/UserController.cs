using BookWeb.Business.Services;
using BookWeb.Business.Services.IServices;
using BookWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IApplicationUserService _userService;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IApplicationUserService userService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }


        #region Call Api
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            foreach (var user in users)
            {
                user.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            }
            return Json(new { data = users });
        }

        #endregion 
    }
}

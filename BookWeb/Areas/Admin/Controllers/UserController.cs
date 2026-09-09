using BookWeb.Business.Services;
using BookWeb.Business.Services.IServices;
using BookWeb.Models;
using BookWeb.Models.ViewModels;
using BookWeb.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace BookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.RoleAdmin + "," + SD.RoleEmployee)]
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

        public async Task<IActionResult> RoleManagement(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return Json(new { success = false, message = "User Not Found" });
            }

            RoleManagementVM RoleVM = new()
            {
                ApplicationUser = user,
                RoleList = _roleManager.Roles.Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Name
                })
            };
            RoleVM.ApplicationUser.Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            return View(RoleVM);
        }

        [HttpPost]
        public async Task<IActionResult> RoleManagement(RoleManagementVM roleManagementVM)
        {
            var user = await _userService.GetUserByIdAsync(roleManagementVM.ApplicationUser.Id);
            if (user == null)
            {
                return Json(new { success = false, message = "User Not Found" });
            }

            string oldRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            if(!(roleManagementVM.ApplicationUser.Role == oldRole))
            {
                //update Role
                await _userManager.RemoveFromRoleAsync(user, oldRole);
                await _userManager.AddToRoleAsync(user, roleManagementVM.ApplicationUser.Role);
            }
            TempData["success"] = "Role has been updated";
            return Redirect(nameof(Index));
        }


        public async Task<IActionResult> ChangePassword(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return Json(new { success = false, message = "User Not Found" });
            }

            AdminChangePasswordVM adminChangePasswordVM = new()
            {
                UserEmail = user.Email,
                UserId = user.Id
            };

            return View(adminChangePasswordVM);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(AdminChangePasswordVM adminChangePasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(adminChangePasswordVM);
            }

            var user = await _userService.GetUserByIdAsync(adminChangePasswordVM.UserId);

            if (user == null)
            {
                return NotFound();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user , token ,adminChangePasswordVM.NewPassword);

            if (result.Succeeded)
            {
                TempData["success"] = $"Password for {user.Email} has been changed successfully";
                return Redirect(nameof(Index));
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            adminChangePasswordVM.UserEmail = user.Email;
            return View(adminChangePasswordVM);
            
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

        [HttpPost]
        public async Task<IActionResult> LockUnlock([FromBody] string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return Json(new { success = false , message = "User Not Found" });

            }
            
            if(await _userManager.IsLockedOutAsync(user))
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow);
                return Json(new { success = true, message = "User unlocked Successfilly" });
            }
            else
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddYears(1000));
                return Json(new { success = true, message = "User locked Successfilly" });
            }
        }

        #endregion 
    }
}

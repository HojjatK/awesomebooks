using System.Threading.Tasks;
using System.Linq;
using AwesomeBooks.Domain.EF;
using AwesomeBooks.Domain.Entities.Identity;
using AwesomeBooks.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AwesomeBooks.Domain.Entities;

namespace AwesomeBooks.Web.Controllers
{   
    [Route("[controller]")]
    public class SetupController : Controller
    {
        private readonly DomainContext _domainContext;
        private UserManager<User> _userManager;
        private RoleManager<Role> _roleManager;

        public SetupController(UserManager<User> userManager,
                              RoleManager<Role> roleManager,
                              DomainContext domainContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _domainContext = domainContext;
        }

        [Route("")]
        public IActionResult Index()
        {   
            return View(new SetupViewModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("Setup")]
        public async Task<IActionResult> Setup(SetupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            // TODO: validation
            if (string.IsNullOrWhiteSpace(model.AdminPassword))
            {
                ModelState.AddModelError(string.Empty, "Email cannot be required.");
                return View("Index", model);
            }

            if (model.AdminPassword != null && !model.AdminPassword.Equals(model.ConfirmAdminPassword))
            {
                ModelState.AddModelError(string.Empty, "Password and Confirm Password should be the same.");
                return View("Index", model);
            }

            var appSettings = _domainContext.AppSettings.FirstOrDefault();
            if (appSettings == null)
            {
                appSettings = new AppSetting
                {
                    Installed = false,
                    Name = "Awesome Books"
                };
                _domainContext.Add(appSettings);
                _domainContext.SaveChanges();
            }

            var succeed = await CreateAdminUser(model);            
            if (succeed)
            {
                appSettings.Installed = true;
                _domainContext.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            // TODO: more meaningful error
            ModelState.AddModelError("", "Invalid information");
            return View("Index", model);
        }

        private async Task<bool> CreateAdminUser(SetupViewModel model)
        {   
            var adminRole = await _roleManager.FindByNameAsync("Administrator");
            if (adminRole == null)
            {
                adminRole = new Role { Name = "Administrator", NormalizedName = "admin" };
                await _roleManager.CreateAsync(adminRole);
            }

            var adminUser = new User { UserName = model.AdminEmail, Email = model.AdminEmail };
            adminUser.PasswordHash = _userManager.PasswordHasher.HashPassword(adminUser, model.AdminPassword);
            await _userManager.CreateAsync(adminUser);

            adminUser.Roles.Add(new UserRole { RoleId = adminRole.Id, UserId = adminUser.Id });
            await _userManager.UpdateAsync(adminUser);
            return true;
            
        }
    }
}

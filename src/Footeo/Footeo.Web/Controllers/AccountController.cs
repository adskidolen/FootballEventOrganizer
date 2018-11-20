namespace Footeo.Web.Controllers
{
    using Footeo.Models;
    using Footeo.Web.ViewModels.Users;
    using Footeo.Web.Controllers.Base;
    using Footeo.Services.Contracts;
    using Footeo.Web.Utilities;

    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Authentication;

    public class AccountController : BaseController
    {
        private readonly UserManager<FooteoUser> userManager;
        private readonly SignInManager<FooteoUser> signInManager;

        private readonly ITownsService townsService;
        private readonly IUsersService usersService;

        public AccountController(UserManager<FooteoUser> userManager, SignInManager<FooteoUser> signInManager,
            ITownsService townsService, IUsersService usersService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;

            this.townsService = townsService;
            this.usersService = usersService;
        }

        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, Constants.InvalidLoginMessage);
                    return View(model);
                }
            }

            return View(model);
        }

        public IActionResult Register(string returnUrl = null)
        {
            this.ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterInputModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var town = this.townsService.GetByName(model.Town);

                if (town == null)
                {
                    town = this.townsService.CreateTown(model.Town);
                }

                var role = model.Role;

                var user = new FooteoUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Age = model.Age,
                    Town = town
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);

                    if (this.userManager.Users.Count() == 1)
                    {
                        var adminResult = userManager.AddToRoleAsync(user, Constants.AdminRoleName).Result;
                    }
                    else
                    {
                        var roleResult = userManager.AddToRoleAsync(user, role).Result;

                        if (role == Constants.PlayerRoleName && this.userManager.Users.Count() > 1)
                        {
                            var player = new Player();
                            this.usersService.CreatePlayer(user, player);
                        }

                        if (role == Constants.RefereeRoleName && this.userManager.Users.Count() > 1)
                        {
                            var referee = new Referee();
                            this.usersService.CreateReferee(user, referee);
                        }
                    }

                    return RedirectToLocal(returnUrl);
                }

                this.AddErrors(result);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
namespace Footeo.Web.Areas.Identity.Pages.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;

    using Footeo.Models;
    using Footeo.Services.Contracts;
    using Footeo.Common;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<FooteoUser> _signInManager;
        private readonly UserManager<FooteoUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ITownsService _townsService;
        private readonly IUsersService _usersService;

        public RegisterModel(
            UserManager<FooteoUser> userManager,
            SignInManager<FooteoUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ITownsService townsService, IUsersService usersService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _townsService = townsService;
            _usersService = usersService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(30, MinimumLength = 3)]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [StringLength(30, MinimumLength = 1)]
            public string FirstName { get; set; }

            [Required]
            [StringLength(30, MinimumLength = 1)]
            public string LastName { get; set; }

            [Required]
            [Range(14, 50)]
            public int Age { get; set; }

            //[Required]
            //public IFormFile Picture { get; set; }

            [Required]
            public string Town { get; set; }

            [Required]
            public string Role { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var town = _townsService.GetTownByName<Town>(Input.Town);

                if (town == null)
                {
                    town = _townsService.CreateTown(Input.Town);
                }

                var role = Input.Role;

                var user = new FooteoUser
                {
                    UserName = Input.Username,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    Age = Input.Age,
                    Town = town
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Users.Count() == 1)
                    {
                        await _userManager.AddToRoleAsync(user, GlobalConstants.AdminRoleName);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, role);

                        if (role == GlobalConstants.PlayerRoleName && _userManager.Users.Count() > 1)
                        {
                            var player = new Player();
                            _usersService.CreatePlayer(user, player);
                        }

                        if (role == GlobalConstants.RefereeRoleName && _userManager.Users.Count() > 1)
                        {
                            var referee = new Referee();
                            _usersService.CreateReferee(user, referee);
                        }
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
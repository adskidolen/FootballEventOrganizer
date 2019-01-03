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
        private readonly IPlayersService _playersService;
        private readonly IRefereesService _refereesService;

        public RegisterModel(
            UserManager<FooteoUser> userManager,
            SignInManager<FooteoUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ITownsService townsService, IPlayersService playersService, IRefereesService refereesService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _townsService = townsService;
            _playersService = playersService;
            _refereesService = refereesService;
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
            [StringLength(30, MinimumLength = 2)]
            [RegularExpression(@"[A-Z]{1}[a-z]+")]
            public string FirstName { get; set; }

            [Required]
            [StringLength(30, MinimumLength = 2)]
            [RegularExpression(@"[A-Z]{1}[a-z]+")]
            public string LastName { get; set; }

            [Required]
            [Range(14, 50)]
            public int Age { get; set; }

            [Required]
            [RegularExpression(@"[A-Z]{1}[a-z]+")]
            public string Town { get; set; }

            [Required]
            public string Role { get; set; }
        }

        public async Task OnGet(string returnUrl = null)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                await this.LocalRedirect("/").ExecuteResultAsync(this.PageContext);
            }

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

                    var role = Input.Role;

                    if (_userManager.Users.Count() == GlobalConstants.UserCountCheckForAdmin)
                    {
                        await _userManager.AddToRoleAsync(user, GlobalConstants.AdminRoleName);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, role);

                        if (role == GlobalConstants.PlayerRoleName)
                        {
                            var player = new Player
                            {
                                FullName = $"{user.FirstName} {user.LastName}"
                            };
                            _playersService.CreatePlayer(user, player);
                        }

                        if (role == GlobalConstants.RefereeRoleName)
                        {
                            var referee = new Referee
                            {
                                FullName = $"{user.FirstName} {user.LastName}"
                            };

                            _refereesService.CreateReferee(user, referee);
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
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Footeo.Common;
using Footeo.Models;
using Footeo.Models.Enums;
using Footeo.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Footeo.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<FooteoUser> _userManager;
        private readonly SignInManager<FooteoUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IPlayersService _playersService;

        public IndexModel(
            UserManager<FooteoUser> userManager,
            SignInManager<FooteoUser> signInManager,
            IEmailSender emailSender,
            IPlayersService playersService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _playersService = playersService;
        }

        public string Username { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            // Player properties
            public string Nickname { get; set; }

            public int SquadNumber { get; set; }

            public string Position { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            if (this.User.IsInRole(GlobalConstants.PlayerRoleName)
                || this.User.IsInRole(GlobalConstants.PlayerInTeamRoleName)
                || this.User.IsInRole(GlobalConstants.CaptainRoleName))
            {
                this.FullName = $"{user.FirstName} {user.LastName}";

                Input = new InputModel
                {
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Nickname = user.Player.Nickname,
                    SquadNumber = user.Player.SquadNumber.GetValueOrDefault(),
                    Position = user.Player.Position.ToString()
                };
            }
            if (this.User.IsInRole(GlobalConstants.RefereeRoleName))
            {
                this.FullName = $"{user.FirstName} {user.LastName}";

                Input = new InputModel
                {
                    Email = email,
                    PhoneNumber = phoneNumber
                };
            }

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            if (this.User.IsInRole(GlobalConstants.PlayerRoleName)
                || this.User.IsInRole(GlobalConstants.PlayerInTeamRoleName)
                || this.User.IsInRole(GlobalConstants.CaptainRoleName))
            {
                _playersService.SetPlayersNickname(user.UserName, Input.Nickname);

                var isSquadNumberTaken = _playersService.IsSquadNumberTaken(Input.SquadNumber);
                if (isSquadNumberTaken)
                {
                    return this.RedirectToPage();
                }

                _playersService.SetSquadNumber(user.UserName, Input.SquadNumber);

                var isPositionValid = Enum.TryParse(typeof(PlayerPosition), Input.Position, out object position);
                if (!isPositionValid)
                {
                    return this.RedirectToPage();
                }

                _playersService.SetPosition(user.UserName, (PlayerPosition)position);
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}

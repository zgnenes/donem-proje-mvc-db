using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace project.Areas.Identity.Pages.Account.Manage {
    public partial class IndexModel : PageModel {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        
        public string Username { get; set; }
        public byte[] ProfilePicture { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync() {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            Email = email;
            PhoneNumber = phoneNumber;

            Input = new InputModel {
                Username = userName,
                Email = email,
                PhoneNumber = phoneNumber
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (Input.Username != Username) {
                var setUsernameResult = await _userManager.SetUserNameAsync(user, Input.Username);
                if (!setUsernameResult.Succeeded) {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting username for user with ID '{userId}'.");
                }
            }

            if (Input.Email != Email) {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded) {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            if (Input.PhoneNumber != PhoneNumber) {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded) {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public class InputModel {
            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Display(Name = "Profile Picture")]
            public byte[] ProfilePicture { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }
    }
}

// Edit.cshtml.cs
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using project.Models;

namespace project.Areas.Identity.Pages.Account.Admin
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfileImage { get; set; }
    }

    public class EditModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public EditModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public IdentityUser UserToEdit { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserToEdit = await _userManager.FindByIdAsync(id);

            if (UserToEdit == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByIdAsync(UserToEdit.Id);
            if (user == null)
            {
                return NotFound();
            }

            user.UserName = UserToEdit.UserName;
            user.Email = UserToEdit.Email;
            user.PhoneNumber = UserToEdit.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
        }
    }

    
}

// Delete.cshtml.cs
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project.Data;
using project.Models;
using Microsoft.AspNetCore.Identity;

namespace project.Areas.Identity.Pages.Account.Admin
{
    public class DeleteModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public DeleteModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IdentityUser UserToDelete { get; set; } // Önceki "User" öğesinin ismi değiştirildi.

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserToDelete = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);

            if (UserToDelete == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            UserToDelete = await _context.Users.FindAsync(id);

            if (UserToDelete != null)
            {
                _context.Users.Remove(UserToDelete);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

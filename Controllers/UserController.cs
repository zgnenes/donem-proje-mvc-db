// UserController.cs

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using project.Data;
using project.Models;
using project.ViewModels;

namespace project.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /User/Profile/{username}
        public IActionResult Profile(string username)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return NotFound();
            }

            var tweets = _context.Tweet.Where(t => t.TwUserName == username).ToList();

            var viewModel = new UserProfileViewModel
            {
                User = user,
                Tweets = tweets
            };

            return View(viewModel);
        }
    }
}

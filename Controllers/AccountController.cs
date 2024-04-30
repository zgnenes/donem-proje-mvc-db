using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using DBProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;


namespace DBProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string sql = "INSERT INTO Users (Username, Email, Password) VALUES (@Username, @Email, @Password)";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Username", model.Username);
                        command.Parameters.AddWithValue("@Email", model.Email);
                        command.Parameters.AddWithValue("@Password", model.Password);

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND Password = @Password";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Email", model.Email);
                    command.Parameters.AddWithValue("@Password", model.Password);

                    connection.Open();
                    object result = command.ExecuteScalar();
                    int count = result == null ? 0 : (int)(long)result;

                    if (count > 0)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid email or password");
                        return View(model);
                    }
                }
            }
        }

        [Authorize] // Sadece yetkilendirilmiş kullanıcılar tarafından erişilebilir
        public async Task<IActionResult> Logout()
        {
            // Kullanıcıyı oturumdan çıkart
            await HttpContext.SignOutAsync();

            // Ana sayfaya yönlendir
            return RedirectToAction("Index", "Home");
        }
    }
}

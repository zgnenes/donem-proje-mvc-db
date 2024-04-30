using System.ComponentModel.DataAnnotations;

namespace DBProject.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; } // Yeni eklendi

        // Diğer özellikler
    }
}

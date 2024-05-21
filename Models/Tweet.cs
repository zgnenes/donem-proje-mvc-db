using System.ComponentModel.DataAnnotations;
using project.ValidationAttributes; // Özel doğrulama özelliğini ekledik

namespace project.Models
{
    public class Tweet
    {
        public int Id { get; set; }

        [PictureUrl(ErrorMessage = "Please enter a valid image URL with .jpg, .jpeg, .png, or .gif extension.")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Please enter a valid title.")]
        public string Text { get; set; }

        public string TwUserName { get; set; } // Kullanıcı adı özelliği
    }
    
}

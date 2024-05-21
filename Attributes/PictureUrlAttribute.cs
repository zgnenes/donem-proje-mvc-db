using System;
using System.ComponentModel.DataAnnotations;

namespace project.ValidationAttributes
{
    public class PictureUrlAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true; // Değer boşsa doğru kabul edilir
            }

            var url = value.ToString();
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                var extension = System.IO.Path.GetExtension(url);
                return Array.Exists(_allowedExtensions, ext => ext.Equals(extension, StringComparison.OrdinalIgnoreCase));
            }

            return false;
        }
    }
}

using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using project.Models;

namespace project.ViewModels
{
    public class UserProfileViewModel
    {
        public IdentityUser User { get; set; }
        public List<Tweet> Tweets { get; set; }
    }
}
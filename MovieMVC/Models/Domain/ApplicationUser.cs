using Microsoft.AspNetCore.Identity;

namespace MovieMVC.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}

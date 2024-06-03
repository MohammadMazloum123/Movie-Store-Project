using System.ComponentModel.DataAnnotations;

namespace MovieMVC.Models.DTO
{
    public class RegisterationModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Username { get; set; }
        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$\r\n")]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        public string Role { get; set; }
    }
}

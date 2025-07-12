using System.ComponentModel.DataAnnotations;

namespace eShopFlix.Models
{
    public class SignupModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cann't be longer then 50 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Role is required")]
        [StringLength(50, ErrorMessage = "Role cann't be longer then 50 characters.")]
        public string Role { get; set; }
    }
}

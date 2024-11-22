using System.ComponentModel.DataAnnotations;

namespace SolarPanelCalculatorApi.Application.DTO
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        public string Password { get; set; }
    }
}

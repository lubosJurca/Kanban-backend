using System.ComponentModel.DataAnnotations;

namespace Kanban_backend.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is required")]
        [MinLength(3, ErrorMessage = "Username must be at least 3 characters")]
        [MaxLength(20, ErrorMessage = "Username cannot exceed 20 characters")]
        
        public string Username { get; set; }
        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6,ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }
    }
}

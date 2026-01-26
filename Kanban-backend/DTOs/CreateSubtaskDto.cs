using System.ComponentModel.DataAnnotations;

namespace Kanban_backend.DTOs
{
    public class CreateSubtaskDto
    {
        [Required]
        [MaxLength(200)]
        [MinLength(3)]
        public required string Description { get; set; }
        public bool Done { get; set; } = false;
    }
}

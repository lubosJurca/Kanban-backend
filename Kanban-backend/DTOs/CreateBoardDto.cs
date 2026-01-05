using System.ComponentModel.DataAnnotations;

namespace Kanban_backend.DTOs

{
    public class CreateBoardDto
    {
        [Required(ErrorMessage = "Title is required")]
        [MinLength(3,ErrorMessage = "Title must have at least 3 characters")]
        [MaxLength(30, ErrorMessage = "Title can't exceed 30 characters")]
        public required string Title { get; set; }
    }
}

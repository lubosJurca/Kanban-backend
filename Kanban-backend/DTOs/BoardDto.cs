namespace Kanban_backend.DTOs
{
    public class BoardDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

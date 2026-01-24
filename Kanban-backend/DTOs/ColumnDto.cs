namespace Kanban_backend.DTOs
{
    public class ColumnDto
    {
        public int Id { get; set; }
        public int BoardId { get; set; }
        public required string Title { get; set; }
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

namespace Kanban_backend.DTOs
{
    public class KanbanTaskDto
    {
        public int Id { get; set; }
        public int ColumnId { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

namespace Kanban_backend.DTOs
{
    public class SubtaskDto
    {
        public int Id { get; set; }
        public int KanbanTaskId { get; set; }
        public required string Description { get; set; }
        public int Order { get; set; }
        public bool Done { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

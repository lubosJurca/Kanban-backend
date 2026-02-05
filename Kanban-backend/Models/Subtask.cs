namespace Kanban_backend.Models
{
    public class Subtask : ITimestampedEntity
    {
        public int Id { get; set; }
        public int KanbanTaskId { get; set; }
        public KanbanTask? KanbanTask { get; set; }
        public required string Description { get; set; }
        public int Order { get; set; }
        public bool Done { get; set; } = false;
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; }
    }
}

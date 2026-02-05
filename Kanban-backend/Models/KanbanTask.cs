namespace Kanban_backend.Models
{
    public class KanbanTask : ITimestampedEntity
    {
        public int Id { get; set; }
        public int ColumnId { get; set; }
        public Column? Column { get; set; }
        public required string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<Subtask> Subtasks { get; set; } = [];
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 
    }
}

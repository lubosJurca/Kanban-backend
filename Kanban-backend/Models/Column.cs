namespace Kanban_backend.Models
{
    public class Column
    {
        public int Id { get; set; }
        public int BoardId { get; set; }
        public Board? Board { get; set; }
        public required string Title { get; set; }
        public List<KanbanTask> KanbanTasks { get; set; } = [];
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; }  
        public DateTime UpdatedAt { get; set; }  
    }
}

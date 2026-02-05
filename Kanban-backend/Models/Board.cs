namespace Kanban_backend.Models
{
    public class Board : ITimestampedEntity
    {
        public int Id { get; set; }
        public int UserId {  get; set;  }
        public User? User { get; set; }
        public required string Title { get; set; }
        public List<Column> Columns { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

namespace Kanban_backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public List<Board> Boards { get; set; } = [];
        public DateTime CreatedAt { get; set; }
    }
}

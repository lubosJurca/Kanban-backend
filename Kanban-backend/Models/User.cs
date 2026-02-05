using Microsoft.AspNetCore.Identity;

namespace Kanban_backend.Models
{
    public class User : IdentityUser<int>, ITimestampedEntity
    {
        public List<Board> Boards { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

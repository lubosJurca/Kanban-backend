using Kanban_backend.Models;

namespace Kanban_backend.Repositories
{
    public interface IBoardRepository
    {
        Task<IEnumerable<Board>> GetAllByUserAsync(int userId);
        Task<Board?> GetBoardByIdAsync(int id);
        Task<Board> CreateBoardAsync(Board board);
        Task<Board?> UpdateBoardAsync(Board board);
        Task<Board?> DeleteBoardAsync(int id);
    }
}

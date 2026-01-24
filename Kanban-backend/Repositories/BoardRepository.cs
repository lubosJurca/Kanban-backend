using Kanban_backend.Data;
using Kanban_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Kanban_backend.Repositories
{
    public class BoardRepository : IBoardRepository
    {
        private readonly AppDbContext _boardContext;

        public BoardRepository(AppDbContext context)
        {
            _boardContext = context;
        }

        public async Task<Board> CreateBoardAsync(Board board)
        {
            board.CreatedAt = DateTime.UtcNow;
            board.UpdatedAt = DateTime.UtcNow;
            _boardContext.Boards.Add(board);
            await _boardContext.SaveChangesAsync();
            return board;
        }

        public async Task<Board?> DeleteBoardAsync(int id)
        {
           var boardToDelete = await _boardContext.Boards.FirstOrDefaultAsync(b => b.Id == id);
             
            if(boardToDelete == null)
            {
                return null;
            }

            _boardContext.Boards.Remove(boardToDelete);
            await _boardContext.SaveChangesAsync();
            return boardToDelete;
        }

        public async Task<IEnumerable<Board>> GetAllByUserAsync(int userId)
        {
            return await _boardContext.Boards
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<Board?> GetBoardByIdAsync(int id)
        {

            return await _boardContext.Boards.FirstOrDefaultAsync(b => b.Id == id);

        }

        public async Task<Board?> UpdateBoardAsync(Board board)
        {
            board.UpdatedAt = DateTime.UtcNow;
            await _boardContext.SaveChangesAsync(); // EF Core is tracking board and makes the update automatically, no need to call Update method

            return board;
        }
    }
}

using Kanban_backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Kanban_backend.Services
{
    public class AuthorizationService : IAuthorizationService
    {

        private readonly AppDbContext _context;

        public AuthorizationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasAccessToBoard(int userId, int boardId)
        { 
            return await _context.Boards
                .AsNoTracking() // Only for read operations, improves performance
                .AnyAsync(b => b.Id == boardId && b.UserId == userId); //Check if board with given ID exists and belongs to user, AnyAsync returns true/false            
        }

        public async Task<bool> HasAccessToColumn(int userId, int columnId)
        {
            return await _context.Columns
                .AsNoTracking()
                .AnyAsync(c => c.Board.UserId == userId && c.Id == columnId); 
        }

        public async Task<bool> HasAccessToKanbanTask(int userId, int kanbanTaskId)
        {
            return await _context.KanbanTasks
                .AsNoTracking()
                .AnyAsync(t => t.Column.Board.UserId == userId && t.Id == kanbanTaskId);
        }

        public async Task<bool> HasAccessToSubtask(int userId, int subtaskId)
        {
            return await _context.Subtasks
                .AsNoTracking()
                .AnyAsync( s => s.KanbanTask.Column.Board.UserId == userId && s.Id == subtaskId);
        }

    }
}

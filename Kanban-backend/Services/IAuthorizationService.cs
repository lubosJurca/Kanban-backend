namespace Kanban_backend.Services
{
    public interface IAuthorizationService
    {
        Task<bool> HasAccessToBoard(int userId, int boardId);
        Task<bool> HasAccessToColumn(int userId, int columnId);
        Task<bool> HasAccessToKanbanTask(int userId, int kanbanTaskId);
        Task<bool> HasAccessToSubtask(int userId, int subtaskId);
    }
}

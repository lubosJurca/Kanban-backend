using Kanban_backend.DTOs;
using Kanban_backend.Models;
using Kanban_backend.Repositories;
using Kanban_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kanban_backend.Controllers
{
    [Route("api/column/{columnId}/kanban-task")]
    [ApiController]
    public class KanbanTaskController : ControllerBase
    {
        private readonly IKanbanTaskRepository _kanbanTaskRepository;
        private readonly IAuthorizationService _authorizationService;
        public KanbanTaskController(IKanbanTaskRepository kanbanTaskRepository, IAuthorizationService authorizationService)
        {
            _kanbanTaskRepository = kanbanTaskRepository;
            _authorizationService = authorizationService;
        }

        // -------------------- GET ALL TASKS BY COLUMN ID --------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KanbanTaskDto>>> GetAllTasksByColumnIdAsync(int columnId) 
        {
            var userId = 1; // To be replaced with actual user identification logic
            var hasAccess = await _authorizationService.HasAccessToColumn(userId, columnId);

            if (!hasAccess)
            {
                return NotFound();
            }

            var tasks = await _kanbanTaskRepository.GetAllByColumnIdAsync(columnId);

            var taskDtos = tasks.Select(task => new KanbanTaskDto
            {
                Id = task.Id,
                ColumnId = task.ColumnId,
                Title = task.Title,
                Description = task.Description,
                Order = task.Order,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            });

            return Ok(taskDtos);
        }

        // -------------------- GET SINGLE TASK BY TASK ID --------------------
        [HttpGet]
        [Route("/api/kanban-task/{kanbanTaskId}", Name = "GetSingleKanbanTaskByIdAsync")] // No need for columnId here
        public async Task<ActionResult<KanbanTaskDto>> GetSingleKanbanTaskByIdAsync(int kanbanTaskId)
        {
            var userId = 1;
            var hasAccess = await _authorizationService.HasAccessToKanbanTask(userId, kanbanTaskId);
            if (!hasAccess) return NotFound();

            var kanbanTask = await _kanbanTaskRepository.GetKanbanTaskByIdAsync(kanbanTaskId);

            var kanbanTaskDto = new KanbanTaskDto
            {
                Id = kanbanTask.Id,
                ColumnId = kanbanTask.ColumnId,
                Title = kanbanTask.Title,
                Description = kanbanTask.Description,
                Order = kanbanTask.Order,
                CreatedAt = kanbanTask.CreatedAt,
                UpdatedAt = kanbanTask.UpdatedAt
            };

            return Ok(kanbanTaskDto);
        }

        // -------------------- CREATE NEW TASK --------------------
        [HttpPost]
        public async Task<ActionResult<KanbanTaskDto>> CreateKanbanTaskAsync(CreateKanbanTaskDto createKanbanTaskDto,int columnId)
        {
            var userId = 1; // To be replaced with actual user identification logic
            var hasAccess = await _authorizationService.HasAccessToColumn(userId, columnId);
            if (!hasAccess) return NotFound();

            var kanbanTask = new KanbanTask
            {
                Title = createKanbanTaskDto.Title,
                Description = createKanbanTaskDto.Description,
                ColumnId = columnId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdKanbanTask = await _kanbanTaskRepository.CreateKanbanTaskAsync(kanbanTask);

            var kanbanTaskDto = new KanbanTaskDto
            {
                Id = createdKanbanTask.Id,
                ColumnId = createdKanbanTask.ColumnId,
                Title = createdKanbanTask.Title,
                Description = createdKanbanTask.Description,
                Order = createdKanbanTask.Order,
                CreatedAt = createdKanbanTask.CreatedAt,
                UpdatedAt = createdKanbanTask.UpdatedAt
            };

            return CreatedAtRoute("GetSingleKanbanTaskByIdAsync", new { kanbanTaskId = kanbanTaskDto.Id }, kanbanTaskDto);
        }

        // -------------------- UPDATE TASK BY ID --------------------
        [HttpPut]
        [Route("/api/kanban-task/{kanbanTaskId}")]
        public async Task<ActionResult<KanbanTaskDto>> UpdateKanbanTaskAsync(int kanbanTaskId, UpdateKanbanTaskDto updateKanbanTaskDto)
        {
            var userId = 1;
            var hasAccess = await _authorizationService.HasAccessToKanbanTask(userId, kanbanTaskId);
            if (!hasAccess) return NotFound();

            var kanbanTask = await _kanbanTaskRepository.GetKanbanTaskByIdAsync(kanbanTaskId);
            
            kanbanTask.Title = updateKanbanTaskDto.Title;
            kanbanTask.Description = updateKanbanTaskDto.Description;

            var updatedKanbanTask = await _kanbanTaskRepository.UpdateKanbanTaskAsync(kanbanTask);

            var kanbanTaskDto = new KanbanTaskDto
            {
                Id = updatedKanbanTask.Id,
                ColumnId = updatedKanbanTask.ColumnId,
                Title = updatedKanbanTask.Title,
                Description = updatedKanbanTask.Description,
                Order = updatedKanbanTask.Order,
                CreatedAt = updatedKanbanTask.CreatedAt,
                UpdatedAt = updatedKanbanTask.UpdatedAt
            };
            return Ok(kanbanTaskDto);

        }

        // -------------------- DELETE TASK BY ID --------------------
        [HttpDelete]
        [Route("/api/kanban-task/{kanbanTaskId}")]
        public async Task<ActionResult<KanbanTaskDto>> DeleteKanbanTaskAsync(int kanbanTaskId)
        {
            var userId = 1;
            var hasAccess = await _authorizationService.HasAccessToKanbanTask(userId, kanbanTaskId);
            if (!hasAccess) return NotFound();
            var deletedKanbanTask = await _kanbanTaskRepository.DeleteKanbanTaskAsync(kanbanTaskId);

            var deletedKanbanTaskDto = new KanbanTaskDto
            {
                Id = deletedKanbanTask.Id,
                ColumnId = deletedKanbanTask.ColumnId,
                Title = deletedKanbanTask.Title,
                Description = deletedKanbanTask.Description,
                Order = deletedKanbanTask.Order,
                CreatedAt = deletedKanbanTask.CreatedAt,
                UpdatedAt = deletedKanbanTask.UpdatedAt
            };

            return Ok(deletedKanbanTaskDto);
        }

        // -------------------- UPDATE TASK ORDER --------------------
        [HttpPut("reorder")]
        public async Task<ActionResult<IEnumerable<KanbanTaskDto>>> ReorderColumnsByIdsAsync(ReorderDto reorderDto,int columnId)
        {
            var userId = 1;
            var hasAccess = await _authorizationService.HasAccessToColumn(userId, columnId);
            if (!hasAccess) return NotFound();
            var reorderedTasks = await _kanbanTaskRepository.Reorder(columnId, reorderDto);

            var reorderedTaskDtos = reorderedTasks.Select(task => new KanbanTaskDto
            {
                Id = task.Id,
                ColumnId = task.ColumnId,
                Title = task.Title,
                Description = task.Description,
                Order = task.Order,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            });

            return Ok(reorderedTaskDtos);
        }
    }
}

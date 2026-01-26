using Kanban_backend.DTOs;
using Kanban_backend.Models;
using Kanban_backend.Repositories;
using Kanban_backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Kanban_backend.Controllers
{
    [Route("api/kanban-task/{kanbanTaskId}/subtask")]
    [ApiController]
    public class SubtaskController : ControllerBase
    {

        private readonly ISubtaskRepository _subtaskRepository;
        private readonly IAuthorizationService _authorizationService;

        public SubtaskController(ISubtaskRepository subtaskRepository, IAuthorizationService authorizationService)
        {
            _subtaskRepository = subtaskRepository;
            _authorizationService = authorizationService;
        }

        // -------------------- GET ALL SUBTASKS BY KANBAN TASK ID --------------------
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubtaskDto>>> GetAllSubtaskByKanbanTaskIdAsync(int kanbanTaskId)
        {
            var userId = 1; // To be replaced with actual user identification logic
            var hasAccess = await _authorizationService.HasAccessToKanbanTask(userId, kanbanTaskId);
            if (!hasAccess) return NotFound();

            var subtasks = await _subtaskRepository.GetAllByKanbanTaskIdAsync(kanbanTaskId);

            var subtasksDto = subtasks.Select(subtask => new SubtaskDto
            {
                Id = subtask.Id,
                KanbanTaskId = subtask.KanbanTaskId,
                Description = subtask.Description,
                Order = subtask.Order,
                Done = subtask.Done,
                CreatedAt = subtask.CreatedAt,
                UpdatedAt = subtask.UpdatedAt
            });

            return Ok(subtasksDto);
        }

        // -------------------- GET SINGLE SUBTASK BY ID --------------------
        [HttpGet]
        [Route("/api/subtask/{subtaskId}", Name = "GetSingleSubtaskByIdAsync")] // No need for kanbanTaskId here
        public async Task<ActionResult<SubtaskDto>> GetSingleSubtaskByIdAsync(int subtaskId)
        {
            var userId = 1;
            var hasAccess = await _authorizationService.HasAccessToSubtask(userId, subtaskId);
            if (!hasAccess) return NotFound();

            var subtask = await _subtaskRepository.GetSubtaskByIdAsync(subtaskId);
            if (subtask == null) return NotFound();

            var subtaskDto = new SubtaskDto
            {
                Id = subtask.Id,
                KanbanTaskId = subtask.KanbanTaskId,
                Description = subtask.Description,
                Order = subtask.Order,
                Done = subtask.Done,
                CreatedAt = subtask.CreatedAt,
                UpdatedAt = subtask.UpdatedAt
            };

            return Ok(subtaskDto);
        }

        // -------------------- CREATE SUBTASK --------------------
        [HttpPost]
        public async Task<ActionResult<SubtaskDto>> CreateSubtaskAsync(int kanbanTaskId, CreateSubtaskDto subtaskDto)
        {
            var userId = 1;
            var hasAccess = await _authorizationService.HasAccessToKanbanTask(userId, kanbanTaskId);
            if (!hasAccess) return NotFound();

            var subtaskToCreate = new Subtask
            {
                KanbanTaskId = kanbanTaskId,
                Description = subtaskDto.Description,
                Done = subtaskDto.Done,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdSubtask = await _subtaskRepository.CreateSubtaskAsync(subtaskToCreate);

            var createdSubtaskDto = new SubtaskDto
            {
                Id = createdSubtask.Id,
                KanbanTaskId = createdSubtask.KanbanTaskId,
                Description = createdSubtask.Description,
                Order = createdSubtask.Order,
                Done = createdSubtask.Done,
                CreatedAt = createdSubtask.CreatedAt,
                UpdatedAt = createdSubtask.UpdatedAt
            };

            return CreatedAtRoute("GetSingleSubtaskByIdAsync", new { subtaskId = createdSubtaskDto.Id }, createdSubtaskDto);
        }

        // -------------------- UPDATE SUBTASK --------------------
        [HttpPut]
        [Route("/api/subtask/{subtaskId}")]
        public async Task<ActionResult<SubtaskDto>> UpdateSubtaskAsync(int subtaskId, UpdateSubtaskDto subtaskDto)
        {
            var userId = 1;
            var hasAccess = await _authorizationService.HasAccessToSubtask(userId, subtaskId);
            if (!hasAccess) return NotFound();

            var existingSubtask = await _subtaskRepository.GetSubtaskByIdAsync(subtaskId);
            if (existingSubtask == null) return NotFound();

            existingSubtask.Description = subtaskDto.Description;
            existingSubtask.Done = subtaskDto.Done;
            var updatedSubtask = await _subtaskRepository.UpdateSubtaskAsync(existingSubtask);
            var updatedSubtaskDto = new SubtaskDto
            {
                Id = updatedSubtask.Id,
                KanbanTaskId = updatedSubtask.KanbanTaskId,
                Description = updatedSubtask.Description,
                Order = updatedSubtask.Order,
                Done = updatedSubtask.Done,
                CreatedAt = updatedSubtask.CreatedAt,
                UpdatedAt = updatedSubtask.UpdatedAt
            };
            return Ok(updatedSubtaskDto);
        }

        // -------------------- DELETE SUBTASK --------------------
        [HttpDelete]
        [Route("/api/subtask/{subtaskId}")]
        public async Task<ActionResult<SubtaskDto>> DeleteSubtaskAsync(int subtaskId)
        {
            var userId = 1;
            var hasAccess = await _authorizationService.HasAccessToSubtask(userId, subtaskId);
            if (!hasAccess) return NotFound();
            var deletedSubtask = await _subtaskRepository.DeleteSubtaskAsync(subtaskId);
            if (deletedSubtask == null) return NotFound();

            var deletedSubtaskDto = new SubtaskDto
            {
                Id = deletedSubtask.Id,
                KanbanTaskId = deletedSubtask.KanbanTaskId,
                Description = deletedSubtask.Description,
                Order = deletedSubtask.Order,
                Done = deletedSubtask.Done,
                CreatedAt = deletedSubtask.CreatedAt,
                UpdatedAt = deletedSubtask.UpdatedAt
            };

            return Ok(deletedSubtaskDto);
        }

        // -------------------- REORDER SUBTASKS --------------------
        [HttpPut("reorder")]
        public async Task<ActionResult<IEnumerable<SubtaskDto>>> ReorderSubtasksAsync(int kanbanTaskId, ReorderDto reorderDto)
        {
            var userId = 1;
            var hasAccess = await _authorizationService.HasAccessToKanbanTask(userId, kanbanTaskId);
            if (!hasAccess) return NotFound();

            var reorderedSubtasks = await _subtaskRepository.Reorder(kanbanTaskId, reorderDto);

            var reorderedSubtasksDto = reorderedSubtasks.Select(subtask => new SubtaskDto
            {
                Id = subtask.Id,
                KanbanTaskId = subtask.KanbanTaskId,
                Description = subtask.Description,
                Order = subtask.Order,
                Done = subtask.Done,
                CreatedAt = subtask.CreatedAt,
                UpdatedAt = subtask.UpdatedAt
            });

            return Ok(reorderedSubtasksDto);
        }
        }
}

using Kanban_backend.Models;
using Kanban_backend.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kanban_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardController : ControllerBase
    {
        private readonly IBoardRepository _boardRepository;
        public BoardController(IBoardRepository boardRepository)
        {
            _boardRepository = boardRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Board>>> GetAllBoards() {
            int userId = 1; // Temporary hardcoded user ID

            //TODO: Replace with actual user ID from authentication context

            var boards = await _boardRepository.GetAllByUserAsync(userId);

            return Ok(boards);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Board>> GetBoardById(int id)
        {
            var board = await _boardRepository.GetBoardByIdAsync(id);

            if (board == null) return NotFound();
          
            return Ok(board);
        }

        [HttpPost]
        public async Task<ActionResult<Board>> CreateBoard(Board board)
        {
            board.UserId = 1; // Temporary hardcoded user ID
            var boardToCreate = await _boardRepository.CreateBoardAsync(board);

            //TODO: Replace with actual user ID from authentication context
            return CreatedAtAction(nameof(GetBoardById), new { id = boardToCreate.Id }, boardToCreate);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Board>> UpdateBoard(int id,Board board)
        {
            if(id != board.Id)
            {
                return BadRequest("Board ID mismatch");
            }

            var updatedBoard = await _boardRepository.UpdateBoardAsync(board);

            if (updatedBoard == null) return NotFound();

            return Ok(updatedBoard);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Board>> DeleteBoardById(int id)
        {
            var deletedBoard = await _boardRepository.DeleteBoardAsync(id);

            if (deletedBoard == null) return NotFound();
            return Ok(deletedBoard);
        }
    }
}

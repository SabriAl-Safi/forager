using Forager.Core.Board;
using Microsoft.AspNetCore.Mvc;

namespace Forager.WebApi.Controllers {
    [Route("api/[controller]")]
    public class GameController : ControllerBase {
        // In-memory storage for demo - use database for production
        private static readonly Dictionary<string, GameState> _activeGames = [];

        public record NewGameRequest(int FieldSize, int NumShrooms, int PcStone);

        [HttpPost("new")]
        public IActionResult CreateNewGame([FromBody] NewGameRequest request) {
            try {
                var gameId = Guid.NewGuid().ToString();
                var gameState = new GameState(request.FieldSize, request.NumShrooms, request.PcStone);

                _activeGames[gameId] = gameState;

                return Ok(new {
                    GameId = gameId,
                    State = GetGameStateResponse(gameState)
                });

            } catch (Exception ex) {
                return BadRequest($"Error creating game: {ex.Message}");
            }
        }

        public record SelectCellRequest(int Row, int Col);

        [HttpPost("{gameId}/action")]
        public IActionResult SelectCell(string gameId, [FromBody] SelectCellRequest request) {
            if (!_activeGames.TryGetValue(gameId, out GameState? gameState))
                return NotFound();

            try {
                // Call your existing game logic
                gameState.Move(request.Row, request.Col);

                return Ok(new {
                    State = GetGameStateResponse(gameState)
                });
            } catch (Exception ex) {
                return BadRequest($"Error processing action: {ex.Message}");
            }
        }

        [HttpPost("{gameId}/reset")]
        public IActionResult ResetGame(string gameId) {
            if (!_activeGames.TryGetValue(gameId, out GameState? gameState))
                return NotFound();

            try {
                // Call your existing game logic
                gameState.Reset();

                return Ok(new {
                    State = GetGameStateResponse(gameState)
                });
            } catch (Exception ex) {
                return BadRequest($"Error processing action: {ex.Message}");
            }
        }

        [HttpDelete("{gameId}")]
        public IActionResult EndGame(string gameId) =>
            _activeGames.Remove(gameId) ? Ok(new { Message = "Game ended" }) : NotFound();

        private static object GetGameStateResponse(GameState gameState) {
            // Convert your game state to a JSON-friendly format
            return new {
                gameState.Cells,
                gameState.NumShroomsFound,
                gameState.CurrentDistance,
                gameState.TargetDistance,
                gameState.IsFinished
            };
        }
    }
}
using Humanizer;
using MemoryGame.Models;
using MemoryGame.Models.Interfaces;
using MemoryGame.Models.Memento;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Packaging;
using System.Text;

namespace MemoryGame.Controllers
{
    public class MemoryBoardController : Controller
    {
        // Static field to hold the game save manager
        public static GameSaveManager? gameSaveManager;

        // Action method for starting a new Pokemon game
        [HttpGet("game/new/pokemon")]
        public async Task<IActionResult> PokemonGameAsync()
        {
            try
            {
                // Subscribe to a new memory board for the Pokemon game
                MemoryBoard memoryBoard = await MemoryBoard.Subscribe("Pokemon");
                // Create a new game save manager with the initial state of the memory board
                gameSaveManager = new GameSaveManager(memoryBoard.TakeSnapshot(), memoryBoard);
                // Pass the current board state to the view
                ViewBag.Board = gameSaveManager.currentBoard;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

            return View("PokemonBoard");
        }

        // Action method for starting a new Pokemon game
        [HttpGet("game/new/pokemonExtreme")]
        public async Task<IActionResult> PokemonGameExtremeAsync()
        {
            try
            {
                // Subscribe to a new memory board for the Pokemon game
                MemoryBoard memoryBoard = await MemoryBoard.Subscribe("PokemonExtreme");
                // Create a new game save manager with the initial state of the memory board
                gameSaveManager = new GameSaveManager(memoryBoard.TakeSnapshot(), memoryBoard);
                // Pass the current board state to the view
                ViewBag.Board = gameSaveManager.currentBoard;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

            return View("PokemonBoard");
        }

        // Action method for starting a new standard game
        [HttpGet]
        [Route("game/new/standard")]
        public async Task<IActionResult> StandardGameAsync()
        {
            // Subscribe to a new memory board for the standard game
            MemoryBoard memoryBoard = await MemoryBoard.Subscribe("Standard");
            // Create a new game save manager with the initial state of the memory board
            gameSaveManager = new GameSaveManager(memoryBoard.TakeSnapshot(), memoryBoard);
            // Pass the current board state to the view
            ViewBag.Board = gameSaveManager.currentBoard;

            return View("StandardBoard");
        }

        // Action method for retrieving the current game state as JSON
        [HttpGet]
        [Route("game/json")]
        public async Task<IActionResult> CurrentGameJson()
        {
            try
            {
                // Serialize the current board state to JSON and return it
                return Ok(JsonConvert.SerializeObject(gameSaveManager.currentBoard, Formatting.Indented));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
            // Return an error response if unable to retrieve the JSON data
            return StatusCode(500, "Unable to retrieve the memory board JSON data");
        }

        // Action method for continuing a standard game
        [HttpGet("game/continue/standard")]
        public IActionResult ContinueStandardGame()
        {
            try
            {
                // Pass the current board state to the view for continuing the game
                ViewBag.Board = gameSaveManager.currentBoard;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

            return View("StandardBoard");
        }

        // Action method for continuing a Pokemon game
        [HttpGet("game/continue/pokemon")]
        public IActionResult ContinuePokemonGame()
        {
            try
            {
                // Pass the current board state to the view for continuing the game
                ViewBag.Board = gameSaveManager.currentBoard;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

            return View("PokemonBoard");
        }

        // Action method for continuing a Pokemon game
        [HttpGet("game/continue/pokemonExtreme")]
        public IActionResult ContinuePokemonGameExtreme()
        {
            try
            {
                // Pass the current board state to the view for continuing the game
                ViewBag.Board = gameSaveManager.currentBoard;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }

            return View("PokemonBoard");
        }

        // Action method for handling card turning requests
        [HttpPost]
        [Route("TurnCards")]
        public async Task<IActionResult> TurnCards()
        {
            int cardId = 0;

            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                string requestBody = await reader.ReadToEndAsync();

                cardId = Int32.Parse(requestBody);
            }

            // Notify the current board about the turned card
            await gameSaveManager.currentBoard.Notify(cardId);

            return Ok();
        }
    }
}

using MemoryGame.Models;
using MemoryGame.Models.Memento;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace MemoryGame.Controllers
{
    public class GameSaveManagerController : Controller
    {
        [HttpGet]
        [Route("view-available-games")]
        public IActionResult SavedGames()
        {
            try
            {
                ViewBag.memoryGamesSavedInMemory = MemoryBoardController.gameSaveManager?.savedMemoryBoards ?? new List<MemoryBoardMemento>();
            }
            catch (Exception ex)
            {
                ViewBag.memoryGamesSavedInMemory = new List<MemoryBoardMemento>();
                Console.WriteLine(ex.Message);
            }
            ViewBag.memoryGamesSavedOnServer = GameSaveManager.RetrieveSavedGamesOnServer();
            return View();
        }


        [HttpPost("/quick-save")]
        public IActionResult QuickSave()
        {
            try
            {
                if (MemoryBoardController.gameSaveManager!.SaveGame() == false)
                {
                    return StatusCode(500, $"Error saving the file!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, $"Error saving the file!");
            }
            // Serialize the current board to JSON
            string jsonString = JsonConvert.SerializeObject(MemoryBoardController.gameSaveManager.currentBoard);

            // Convert the JSON string to bytes
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonString);

            // Create a MemoryStream from the byte array
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                // Specify the file path for the new file on the server
                string currentDateTime = DateTime.Now.ToString("yyyy_MM_dd_HHmmss");
                string folder = MemoryBoardController.gameSaveManager.currentBoard.gameType.Equals("Pokemon") || MemoryBoardController.gameSaveManager.currentBoard.gameType.Equals("PokemonExtreme") ? "pokemon" : "standard";
                string fileName = $"saved_games/{folder}/Memorygame_{MemoryBoardController.gameSaveManager.currentBoard.gameType}_{currentDateTime}.json";
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);


                try
                {
                    // Write the MemoryStream to a new file on the server
                    using (FileStream fileStream = new FileStream(filePath, FileMode.CreateNew))
                    {
                        stream.CopyTo(fileStream);
                    }
                    return StatusCode(200, $"Succesfully saved the file - {MemoryBoardController.gameSaveManager.savedMemoryBoards.Count}");
                }
                catch (Exception ex)
                {
                    // Handle the exception if necessary
                    // Log or perform additional error handling
                    Console.WriteLine(ex.Message);
                    return StatusCode(500, $"Error creating and saving the file: {ex.Message}");

                }
            }
        }

        [HttpPost("/load-game")]
        public IActionResult LoadGame([FromForm] string Loader)
        {
            try
            {
                MemoryBoardMemento board = JsonConvert.DeserializeObject<MemoryBoardMemento>(Loader)!;

                if (MemoryBoardController.gameSaveManager == null)
                {
                    MemoryBoard newBoard = new MemoryBoard(board.cards, board.score1, board.score2, board.gameType, board.DrawnMemoryCard, board.currentPlayer);
                    MemoryBoardController.gameSaveManager = new GameSaveManager(board, newBoard);

                    if (string.Equals(board.gameType, "Pokemon", StringComparison.OrdinalIgnoreCase))
                    {
                        return Redirect("/game/continue/pokemon");
                    }
                    if (string.Equals(board.gameType, "PokemonExtreme", StringComparison.OrdinalIgnoreCase))
                    {
                        return Redirect("/game/continue/pokemonExtreme");
                    }
                    else
                    {
                        return Redirect("/game/continue/standard");
                    }
                }

                if (MemoryBoardController.gameSaveManager!.LoadGame(board))
                {
                    if (string.Equals(board.gameType, "Pokemon", StringComparison.OrdinalIgnoreCase))
                    {
                        return Redirect("/game/continue/pokemon");
                    }
                    if (string.Equals(board.gameType, "PokemonExtreme", StringComparison.OrdinalIgnoreCase))
                    {
                        return Redirect("/game/continue/pokemonExtreme");
                    }
                    else
                    {
                        return Redirect("/game/continue/standard");
                    }
                }

                return StatusCode(500, "Something went wrong when restoring the memoryboard!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500, $"Error processing the request: {ex.Message}");
            }
        }

    }
}

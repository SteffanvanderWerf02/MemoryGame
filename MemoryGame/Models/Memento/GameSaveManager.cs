using MemoryGame.Models.Interfaces;
using Newtonsoft.Json.Linq;
using MemoryGame.Models.Cards;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MemoryGame.Models.Memento
{
    public class GameSaveManager
    {
        // memoryboard that holds the actual state of the memory board
        public MemoryBoard currentBoard { get; set; }

        // list of saved games, and last saved game
        public MemoryBoardMemento savedMemoryBoard { get; set; }
        public List<MemoryBoardMemento> savedMemoryBoards = new List<MemoryBoardMemento>();

        // constructor for intializing the memento memoryboard
        public GameSaveManager(MemoryBoardMemento memoryBoardMemento, MemoryBoard memoryBoard)
        {
            this.currentBoard = memoryBoard;
            this.savedMemoryBoard = memoryBoardMemento;
        }

        /// <summary>
        /// create a snapshot and update the caretaker class variables
        /// </summary>
        /// <returns> true of false, based on either it was added to the list </returns>
        public bool SaveGame()
        {
            try {
                MemoryBoardMemento memoryBoardMemento = this.currentBoard.TakeSnapshot();
                int oldSavedMemoryBoardsLength = this.savedMemoryBoards.Count;
                this.savedMemoryBoards.Add(memoryBoardMemento);
                this.savedMemoryBoard = memoryBoardMemento;

                // check if it was succesfully added to the saved memory boards
                if (this.savedMemoryBoards.Count > oldSavedMemoryBoardsLength)
                {
                    return true;
                }
                return false;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }   

        /// <summary>
        /// Restore a new game to the currentboard
        /// </summary>
        /// <param name="board">The requested memento to be loaded in</param>
        /// <returns>returns true or false, either if the restoring was succesfull or not</returns>
        public bool LoadGame(MemoryBoardMemento board)
        {
            return currentBoard.Restore(board);
        }

        /// <summary>
        /// retrieve all the games that are saved on the server as memento objects 
        /// </summary>
        /// <returns> List of memoryboard memento's generated from the .json files saved on the server </returns>
        public static List<MemoryBoardMemento> RetrieveSavedGamesOnServer()
        {

            // Specify the path to the folder containing .json files
            string pokemonFolderPath = "saved_games/pokemon";
            string standardFolderPath = "saved_games/standard";

            // Get all .json files in the folder
            string[] pokemonMemoryJsonFiles = Directory.GetFiles(pokemonFolderPath, "*.json");
            string[] standardMemoryJsonFiles = Directory.GetFiles(standardFolderPath, "*.json");

            //list for mementos
            List<MemoryBoardMemento> savedMementos = new List<MemoryBoardMemento>();

            // read all contents of the .json files in both directories
            foreach (string filePath in pokemonMemoryJsonFiles)
            {
                string content = ReadFileContent(filePath);
                savedMementos.Add(JsonConvert.DeserializeObject<MemoryBoardMemento>(content)!);
            }
           
            foreach (string filePath in standardMemoryJsonFiles)
            {
                string content = ReadFileContent(filePath);
                savedMementos.Add(JsonConvert.DeserializeObject<MemoryBoardMemento>(content)!);
            }
            
            savedMementos.Reverse();
            return savedMementos;
        }

        /// <summary>
        /// read the content of a file
        /// </summary>
        /// <param name="filePath"> path the the file </param>
        /// <returns></returns>
        public static string ReadFileContent(string filePath)
        {
            try
            {
                string content = File.ReadAllText(filePath);
                return content;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file {filePath}: {ex.Message}");
                return string.Empty;
            }
        }
    }
}

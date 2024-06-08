using MemoryGame.Models.Factory;
using MemoryGame.Models.Interfaces;
using MemoryGame.Models.Memento;
using MemoryGame.Tools.CardDeserilizer;
using MemoryGame.Tools.ListExtention;
using Newtonsoft.Json;
using System.Diagnostics;

namespace MemoryGame.Models
{
    public class MemoryBoard
    {
        // Score of player 1 and player 2
        public int score1 { set; get; }
        public int score2 { set; get; }
        public int currentPlayer { set; get; }
        public bool inUse = false;

        // Currently drawn memory card
        [JsonConverter(typeof(MemoryCardDeserilizer))]
        public IMemoryCard? DrawnMemoryCard { get; set; }

        // List of cards implementing the IMemoryCard interface
        [JsonConverter(typeof(MemoryCardListDeserilizer))]
        public List<IMemoryCard> cards { set; get; }

        // Type of game, either "pokemon" or "standard"
        public string gameType { set; get; }

        public int gridSize = 8; // Size of the grid (8x8)
        public int rowCount; // Number of rows

        // Serialize the current class to a JSON object 
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        // Constructor
        public MemoryBoard(List<IMemoryCard> cards, int score1, int score2, string gameType, IMemoryCard? drawnCard, int player)
        {
            this.score1 = score1;
            this.score2 = score2;
            this.gameType = gameType;
            this.cards = cards;
            this.rowCount = cards.Count / gridSize;
            this.DrawnMemoryCard = drawnCard;
            this.currentPlayer = player;
        }

        /// <summary>
        /// Create the memory board with the specified game type, either "pokemon" or "standard" 
        /// </summary>
        /// <param name="gameType">Type of game, either "pokemon" or "standard"</param>
        public static async Task<MemoryBoard> Subscribe(string gameType)
        {
            List<IMemoryCard> cards;

            if (gameType.Equals("pokemon", StringComparison.OrdinalIgnoreCase) || gameType.Equals("PokemonExtreme", StringComparison.OrdinalIgnoreCase))
            {
                cards = await MemoryCardFactory.CreatePokemonMemoryCardsAsync();
            }
            else
            {
                cards = MemoryCardFactory.CreateStandardMemoryCards();
            }

            cards.Shuffle();

            return new MemoryBoard(cards, 0, 0, gameType, null , 0);
        }

        /// <summary>
        /// Take a snapshot of the entire board including the cards
        /// </summary>
        /// <returns>A new memento object</returns>
        public MemoryBoardMemento TakeSnapshot()
        {
            try
            {
                return new MemoryBoardMemento(this.cards, this.score1, this.score2, this.gameType, this.DrawnMemoryCard, this.currentPlayer);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Restore a memento to the current class
        /// </summary>
        /// <param name="memoryBoardMemento"></param>
        /// <returns>True if it was restored correctly, otherwise false</returns>
        public bool Restore(MemoryBoardMemento memoryBoardMemento)
        {
            try
            {
                this.score1 = memoryBoardMemento.score1;
                this.score2 = memoryBoardMemento.score2;
                this.gameType = memoryBoardMemento.gameType;
                this.cards = memoryBoardMemento.cards;
                this.DrawnMemoryCard = memoryBoardMemento.DrawnMemoryCard;
                this.currentPlayer = memoryBoardMemento.currentPlayer;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Notifies the card about a click
        /// </summary>
        /// <param name="cardId">The ID of the card being turned.</param>
        public async Task Notify(int cardId)
        {
            bool extremeSetting = false;

            // Check if the board is currently in use
            if (!inUse)
            {
                // Set the board in use
                inUse = true;

                // Iterate over each card in the board
                foreach (IMemoryCard card in cards)
                {
                    // Compare cards based on the provided cardId
                    if (card.IsSameCard(cardId))
                    {
                        // Check if no card is currently drawn
                        if (DrawnMemoryCard == null)
                        {
                            // Update the card state to drawn and set it as the drawn card
                            card.UpdateIsDrawn(true);
                            DrawnMemoryCard = card;
                            inUse = false;
                        }
                        else
                        {
                            // Update the current card state to drawn
                            card.UpdateIsDrawn(true);

                            // Check if the card ID is different from DrawnMemoryCard
                            if (cardId != DrawnMemoryCard.cardId)
                            {
                                // Check if the memory IDs of the two cards match
                                if (card.memoryId == DrawnMemoryCard.memoryId)
                                {
                                    // Update both cards as matched
                                    card.UpdateIsMatched(true);
                                    DrawnMemoryCard.UpdateIsMatched(true);

                                    // Reset DrawnMemoryCard
                                    DrawnMemoryCard = null;

                                    // Update the scores and continue the game
                                    UpdateScore(false);

                                    // Release the Notify
                                    inUse = false;
                                }
                                else
                                {
                                    // Wait for 1 second before flipping the cards back
                                    await Task.Delay(1000);

                                    // Reset the drawn cards and update the scores
                                    card.UpdateIsDrawn(false);

                                    // if DrawnMemoryCard is set find the card and updateDrawn to false
                                    if (DrawnMemoryCard != null)
                                    { 
                                        foreach (IMemoryCard drawnCard in this.cards)
                                        {
                                            if (drawnCard.cardId == DrawnMemoryCard.cardId)
                                            {
                                                drawnCard.UpdateIsDrawn(false);
                                                break;
                                            }
                                        }
                                    }
                                    DrawnMemoryCard = null;

                                    // Update the scores
                                    UpdateScore(true);

                                    // Release the board
                                    inUse = false;
                                }

                                if (this.gameType.Equals("PokemonExtreme", StringComparison.OrdinalIgnoreCase))
                                {
                                    // if matching set shuffle on true
                                    extremeSetting = true;
                                }
                            }
                        }
                    }
                }

                // Shuffle the cards outside of the foreach loop to avoid expetions
                if (extremeSetting)
                {
                    this.cards.Shuffle();
                    await Task.Delay(1000);
                }
            }
        }


        /// <summary>
        /// Update the game scores and current player
        /// </summary>
        /// <param name="goToNextPlayer">Indicates if it's time to switch players</param>
        public void UpdateScore(bool goToNextPlayer)
        {
            if (currentPlayer == 0)
            {
                if (goToNextPlayer)
                {
                    currentPlayer++;
                }
                else
                {
                    score1++;
                }
            }
            else
            {
                if (goToNextPlayer)
                {
                    currentPlayer--;
                }
                else
                {
                    score2++;
                }
            }
        }
    }
}

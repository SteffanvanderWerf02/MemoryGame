using MemoryGame.Models.Interfaces;
using Newtonsoft.Json;

namespace MemoryGame.Models.Cards
{
    /// <summary>
    /// Represents a Pokemon-themed memory card.
    /// </summary>
    public class PokemonCard : IMemoryCard
    {
        private static int lastCardId = 0;
        public int cardId { set; get; }
        public int memoryId { set; get; }

        public bool isMatched { set; get; }

        public bool isDrawn { set; get; }

        public string imagePathFront { set; get; }
        public string ToJson
        {
            get
            {
                return $"{{cardId:{cardId},memoryId:{memoryId},isMatched:{isMatched},isDrawn:{isDrawn},imagePathFront:{imagePathFront}}}";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PokemonCard"/> class with the specified parameters.
        /// </summary>
        /// <param name="memoryId">The ID representing the type of memory the card holds.</param>
        /// <param name="imagePathFront">The file path to the image displayed on the front side of the card.</param>
        public PokemonCard(int memoryId, string imagePathFront)
        {
            this.memoryId = memoryId;
            this.cardId = GenerateUniqueId();
            this.imagePathFront = imagePathFront;
        }

        /// <summary>
        /// Constructor used for deserialization for the memento pattern.
        /// </summary>
        [JsonConstructor]
        public PokemonCard(int cardId, int memoryId, bool isMatched, bool isDrawn, string imagePathFront)
        {
            this.cardId = cardId;
            this.memoryId = memoryId;
            this.isMatched = isMatched;
            this.isDrawn = isDrawn;
            this.imagePathFront = imagePathFront;
        }

        /// <summary>
        /// Generates a unique identifier for the card.
        /// </summary>
        /// <returns>A unique identifier for the card.</returns>
        private int GenerateUniqueId()
        {
            return Interlocked.Increment(ref lastCardId); // Atomically increment the ID and return it
        }

        /// <summary>
        /// Checks if the provided card ID is the same as this card's ID.
        /// </summary>
        /// <param name="cardId">The card ID to compare.</param>
        /// <returns>True if the card IDs are the same; otherwise, false.</returns>
        public bool IsSameCard(int cardId)
        {
            return this.cardId.Equals(cardId);
        }

        /// <summary>
        /// Updates the state of the card to indicate whether it has been drawn.
        /// </summary>
        /// <param name="mode">True to indicate the card has been drawn; otherwise, false.</param>
        public void UpdateIsDrawn(bool mode)
        {
            this.isDrawn = mode;
        }

        /// <summary>
        /// Updates the state of the card to indicate whether it has been matched.
        /// </summary>
        /// <param name="mode">True to indicate the card has been matched; otherwise, false.</param>
        public void UpdateIsMatched(bool mode)
        {
            this.isMatched = mode;
        }
    }
}

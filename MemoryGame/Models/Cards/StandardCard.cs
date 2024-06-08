using MemoryGame.Models.Interfaces;
using Newtonsoft.Json;

namespace MemoryGame.Models.Cards
{
    /// <summary>
    /// Represents a standard-themed memory card.
    /// </summary>
    public class StandardCard : IMemoryCard
    {
        private static int lastCardId = 0;
        public int cardId { set; get; }
        public int memoryId { set; get; }
        public bool isMatched { set; get; }
        public bool isDrawn { set; get; }
        public int cardValue { set; get; }
        public string ToJson
        {
            get
            {
                return $"{{cardId:{cardId},memoryId:{memoryId},isMatched:{isMatched},isDrawn:{isDrawn},cardValue:{cardValue}}}";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StandardCard"/> class with the specified card value.
        /// </summary>
        /// <param name="cardValue">The value represented by the card.</param>
        public StandardCard(int cardValue)
        {
            this.cardValue = cardValue;
            this.memoryId = cardValue;
            this.cardId = GenerateUniqueId();
        }

        /// <summary>
        /// Constructor used for deserialization for the memento pattern.
        /// </summary>
        [JsonConstructor]
        public StandardCard(int cardId, int memoryId, bool isMatched, bool isDrawn, int cardValue)
        {
            this.cardId = cardId;
            this.memoryId = memoryId;
            this.isMatched = isMatched;
            this.isDrawn = isDrawn;
            this.cardValue = cardValue;
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

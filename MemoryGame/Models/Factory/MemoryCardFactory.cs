using MemoryGame.Models.Cards;
using MemoryGame.Models.Interfaces;
using PokeApiNet;
namespace MemoryGame.Models.Factory
{
    public static class MemoryCardFactory
    {
        /// <summary>
        /// Generate PokemonCards Async
        /// </summary>
        /// <returns>Returns List with memory cards</returns>
        public static async Task<List<IMemoryCard>> CreatePokemonMemoryCardsAsync()
        {
            List<IMemoryCard> generatedCards = new List<IMemoryCard>();
            try
            {
                PokeApiClient pokeClient = new PokeApiClient();
                List<int> pokemonNumbers = GenerateRandomNumbers();

                foreach (int pokemonNumber in pokemonNumbers)
                {
                    Pokemon pokemon = await pokeClient.GetResourceAsync<Pokemon>(pokemonNumber);
                    for (int set = 0; set < 2; set++)
                    {
                        IMemoryCard card = new PokemonCard(pokemon.Id, pokemon.Sprites.Other.OfficialArtwork.FrontShiny);

                        generatedCards.Add(card);
                    }
                }
            }
                catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return generatedCards;
        }

        /// <summary>
        /// Generates normal cards with value
        /// </summary>
        /// <returns>Returns list with cards</returns>
        public static List<IMemoryCard> CreateStandardMemoryCards()
        {
            List<IMemoryCard> generatedCards = new List<IMemoryCard>();
            List<int> cardAmountNeeded= GenerateRandomNumbers();

            foreach (var cardValue in cardAmountNeeded)
            {
                for (int set = 0; set < 2; set++) {
                    IMemoryCard card = new StandardCard(cardValue); 
                    generatedCards.Add(card);
                }
            }

            return generatedCards;
        }

        /// <summary>
        /// Generate Random numer that is used to select a Pokemon and generate random value
        /// </summary>
        /// <returns>Returns list with random values</returns>
        private static List<int> GenerateRandomNumbers() {
            List<int> existingNumbers = new List<int>();
            List<int> result = new List<int>();
            Random random = new Random();

            while (result.Count < 8)
            {
                int newNumber = random.Next(1, 1026); // Generating a random number between 1 and 1025 because there 1025 pokemons
                if (!existingNumbers.Contains(newNumber))
                {
                    result.Add(newNumber);
                    existingNumbers.Add(newNumber);
                }
            }

            return result;

        }
    }
}

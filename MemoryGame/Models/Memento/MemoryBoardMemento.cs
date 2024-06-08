using MemoryGame.Models.Interfaces;
using MemoryGame.Tools.CardDeserilizer;
using Newtonsoft.Json;


// memento for DP class
namespace MemoryGame.Models.Memento
{
    public class MemoryBoardMemento
    {
        // score of p1 and p2
        public int score1 {set;get;}
        public int score2 {set;get;}

        // type of game, either pokemon or standard
        public string gameType {set;get;}
        // list of cards that implement the IMemoryCard interface
        [JsonConverter(typeof(MemoryCardListDeserilizer))]
        public List<IMemoryCard> cards {set;get;}

        [JsonConverter(typeof(MemoryCardDeserilizer))]
        public IMemoryCard? DrawnMemoryCard { set;get;}

        public int currentPlayer { set; get; }

        // convert the current object to JSON
        public string ToJson(){
            return JsonConvert.SerializeObject(this);
        }

        public MemoryBoardMemento(List<IMemoryCard> cards, int score1, int score2, string gameType, IMemoryCard? drawnCard, int player)
        {
            this.cards = new List<IMemoryCard>(cards); 
            this.score1 = score1;
            this.score2 = score2;
            this.gameType = gameType;
            this.DrawnMemoryCard = drawnCard;
            this.currentPlayer = player;

        }
    }
}

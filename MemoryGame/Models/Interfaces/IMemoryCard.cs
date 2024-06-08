namespace MemoryGame.Models.Interfaces
{
    public interface IMemoryCard
    {
        public int cardId { set; get; }
        public int memoryId { set; get; }
        public bool isMatched { set; get; }
        public bool isDrawn { set; get; }
        public bool IsSameCard(int cardId);
        public void UpdateIsDrawn(bool mode);
        public void UpdateIsMatched(bool mode);
        public string ToJson
        {
            get;
        }
    }
}

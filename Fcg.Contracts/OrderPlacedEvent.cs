namespace Fcg.Contracts
{
    public class OrderPlacedEvent
    {
        public string Id { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string GameId { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}

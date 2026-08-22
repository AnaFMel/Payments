namespace Fcg.Contracts
{
    public class OrderPlacedEvent
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string GameId { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public OrderPlacedEvent(int userId, string userEmail, string gameId, decimal price)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            UserEmail = userEmail;
            GameId = gameId;
            Price = price;
        }
    }
}

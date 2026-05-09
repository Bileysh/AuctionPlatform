namespace AuctionPlatform.Domain.Entities;

public class AuctionItem
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal StartingPrice { get; private set; }
    public decimal CurrentPrice { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime EndsAt { get; private set; }
    public Guid SellerId { get; private set; }
    public Guid? WinnerId { get; private set; }
    
    public uint Version { get; private set; }
    
    public User Seller { get; private set; } = null!;
    public User? Winner { get; private set; }
    
    protected AuctionItem(){}
    
    public AuctionItem(string title, string description, decimal startingPrice, DateTime endsAt, Guid sellerId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        StartingPrice = startingPrice;
        CurrentPrice = startingPrice;
        CreatedAt = DateTime.UtcNow;
        EndsAt = endsAt;
        SellerId = sellerId;
        Version = 0;
    }
    
    public void UpdatePriceAndWinner(decimal newPrice, Guid bidderId)
    {
        if (newPrice <= CurrentPrice)
            throw new Exception("Bid amount must be greater than the current price.");
            
        if (DateTime.UtcNow > EndsAt)
            throw new Exception("This auction is already closed.");

        CurrentPrice = newPrice;
        WinnerId = bidderId;
    }
}
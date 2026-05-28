using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionPlatform.Domain.Entities;

public class Bid
{
    public Guid Id { get; private set; }
    public Guid AuctionItemId { get; private set; }
    public Guid BidderId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    [ForeignKey(nameof(AuctionItemId))]
    public AuctionItem AuctionItem { get; private set; } = null!;
    [ForeignKey(nameof(BidderId))]
    public User Bidder { get; private set; } = null!;
    
    protected Bid(){}
    
    public Bid(Guid auctionItemId, Guid bidderId, decimal amount)
    {
        Id = Guid.NewGuid();
        AuctionItemId = auctionItemId;
        BidderId = bidderId;
        Amount = amount;
        CreatedAt = DateTime.UtcNow;
    }
}
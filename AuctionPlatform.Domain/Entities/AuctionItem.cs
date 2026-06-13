using AuctionPlatform.Domain.Entities.Enums;
using AuctionPlatform.Domain.Entities.Interfaces;

namespace AuctionPlatform.Domain.Entities;

public class AuctionItem: ISoftDeletable
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
    public AuctionStatus Status { get; private set; }
    public int CategoryId { get; private set; }
    public bool IsDeleted { get; set; }
    
    public User Seller { get; private set; } = null!;
    public User? Winner { get; private set; }
    public Category Category { get; private set; } = null!;
    public ICollection<Bid> Bids { get; private set; } = new List<Bid>();
    public ICollection<Comment> Comments { get; private set; } = new List<Comment>();
    
    protected AuctionItem(){}
    
    public AuctionItem(string title, string description, decimal startingPrice, DateTime endsAt, Guid sellerId, int categoryId)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        StartingPrice = startingPrice;
        CurrentPrice = startingPrice;
        EndsAt = endsAt;
        CreatedAt = DateTime.UtcNow;
        SellerId = sellerId;
        CategoryId = categoryId; 
        Status = AuctionStatus.Active; 
    }

    public void Cancel()
    {
        if (Status != AuctionStatus.Active)
            throw new Exception("Only active auctions can be canceled.");
            
        if (Bids.Any())
            throw new Exception("Cannot cancel auction because bids have already been placed.");

        Status = AuctionStatus.Cancelled;
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
    
    public void UpdateDetails(string title, string? description, DateTime endsAt, int categoryId)
    {
        if (Status != AuctionStatus.Active)
            throw new Exception("Only active auctions can be updated.");
            
        if (DateTime.UtcNow > EndsAt)
            throw new Exception("Cannot update auction because it has already ended.");
        
        if (Bids.Any())
            throw new Exception("Cannot update auction details because bids have already been placed.");
       
        Title = title;
        Description = description;
        EndsAt = endsAt;
        CategoryId = categoryId;
    }
    
    public void Close()
    {
        if (Status != AuctionStatus.Active)
            throw new Exception("Auction is already closed or canceled.");
            
        Status = AuctionStatus.Finished;
    }
}
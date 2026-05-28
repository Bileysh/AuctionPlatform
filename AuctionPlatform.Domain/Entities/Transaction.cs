using AuctionPlatform.Domain.Entities.Enums;

namespace AuctionPlatform.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public decimal Amount { get; private set; } 
    public TransactionType Type { get; private set; }
    public Guid? ReferenceId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public User User { get; private set; } = null!;
    
    protected Transaction(){}

    public Transaction(Guid userId, decimal amount, TransactionType type, Guid? referenceId = null)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Amount = amount;
        Type = type;
        ReferenceId = referenceId;
        CreatedAt = DateTime.UtcNow;
    }
}
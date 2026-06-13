using AuctionPlatform.Domain.Entities.Enums;
using AuctionPlatform.Domain.Entities.Interfaces;

namespace AuctionPlatform.Domain.Entities;

public class Transaction: ISoftDeletable
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public decimal Amount { get; private set; } 
    public TransactionType Type { get; private set; }
    public Guid? ReferenceId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public User User { get; private set; } = null!;
    public bool IsDeleted { get; set; }
    protected Transaction(){}

    public Transaction(Guid userId, decimal amount, TransactionType type, Guid? referenceId = null)
    {
        UserId = userId;
        Amount = amount;
        Type = type;
        ReferenceId = referenceId;
        CreatedAt = DateTime.UtcNow;
    }
}
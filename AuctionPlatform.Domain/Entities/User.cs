using AuctionPlatform.Domain.Entities.Enums;

namespace AuctionPlatform.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Auth0Id { get; private set; } = string.Empty;
    public string UserName { get; private set; } = string.Empty;
    public decimal Balance { get; private set; }
    
    public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();
    protected User(){}
    
    public User(string auth0Id, string userName)
    {
        Id = Guid.NewGuid();
        Auth0Id = auth0Id;
        UserName = userName;
        Balance = 0;
    }
    
    public decimal GetAvailableBalance()
    {
        var heldAmount = Transactions
            .Where(t => t.Type == TransactionType.Hold)
            .Sum(t => t.Amount);
        
        var releasedAmount = Transactions
            .Where(t => t.Type == TransactionType.Release || t.Type == TransactionType.Payment)
            .Sum(t => t.Amount);
        
        var currentHeld = heldAmount - releasedAmount;
        
        return Balance - currentHeld;
    }
    
    public Transaction Deposit(decimal amount)
    {
        if (amount <= 0) throw new Exception("Deposit amount must be positive.");
        
        Balance += amount;
        var tx = new Transaction(Id, amount, TransactionType.Deposit);
        Transactions.Add(tx);
        return tx;
    }
    
    public Transaction HoldFunds(decimal amount, Guid auctionId)
    {
        if (amount <= 0) throw new Exception("Hold amount must be positive.");
        if (GetAvailableBalance() < amount) throw new Exception("Insufficient available funds to place this bid.");

        var tx = new Transaction(Id, amount, TransactionType.Hold, auctionId);
        Transactions.Add(tx);
        return tx;
    }
    
    public Transaction ReleaseFunds(decimal amount, Guid auctionId)
    {
        var tx = new Transaction(Id, amount, TransactionType.Release, auctionId);
        Transactions.Add(tx);
        return tx;
    }
    
    public void UpdateProfile(string newUserName)
    {
        if (string.IsNullOrWhiteSpace(newUserName))
            throw new Exception("Username cannot be empty.");
        
        UserName = newUserName;
    }
}
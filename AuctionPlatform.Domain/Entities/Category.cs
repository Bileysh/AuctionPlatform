using AuctionPlatform.Domain.Exceptions;

namespace AuctionPlatform.Domain.Entities;

public class Category
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    
    public ICollection<AuctionItem> Auctions { get; private set; } = new List<AuctionItem>();
    
    protected Category(){}
    
    public Category(string name)
    {
        Name = name;
    }
    
    public void UpdateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new BusinessRuleException("Category name cannot be empty.");
        
        Name = newName;
    }
}
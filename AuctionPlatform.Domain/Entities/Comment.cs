namespace AuctionPlatform.Domain.Entities;

public class Comment
{
    public Guid Id { get; private set; }
    public Guid AuctionItemId { get; private set; }
    public Guid AuthorId { get; private set; }
    public string Text { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    
    public AuctionItem AuctionItem { get; private set; } = null!;
    public User Author { get; private set; } = null!;
    
    protected Comment(){}
    
    public Comment(Guid auctionItemId, Guid authorId, string text)
    {
        Id = Guid.NewGuid();
        AuctionItemId = auctionItemId;
        AuthorId = authorId;
        Text = text;
        CreatedAt = DateTime.UtcNow;
    }
}
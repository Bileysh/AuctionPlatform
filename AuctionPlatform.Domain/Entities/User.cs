namespace AuctionPlatform.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Auth0Id { get; private set; } = string.Empty;
    public string UserName { get; private set; } = string.Empty;
    public decimal Balance { get; private set; }
    
    protected User(){}
    
    public User(string auth0Id, string userName)
    {
        Id = Guid.NewGuid();
        Auth0Id = auth0Id;
        UserName = userName;
        Balance = 0;
    }
}
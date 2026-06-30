namespace AuctionPlatform.Application.Users.Queries.GetUserById;

public record UserDto(
    Guid Id, 
    string Username, 
    string? Auth0Id, 
    decimal TotalBalance, 
    decimal AvailableBalance);
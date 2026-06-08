namespace AuctionPlatform.Application.Users.Queries;

public record UserDto(
    Guid Id, 
    string Username, 
    string Auth0Id, 
    decimal TotalBalance, 
    decimal AvailableBalance);
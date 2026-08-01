namespace AuctionPlatform.Application.Users.Queries.GetUserById;

public record UserDto(
    Guid Id, 
    string Username, 
    string? Auth0Id = null, 
    decimal? TotalBalance = null, 
    decimal? AvailableBalance = null
);
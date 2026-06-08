namespace AuctionPlatform.WebApi.DTO;

public record UpdateAuctionRequest(string Title, string Description, DateTime EndsAt, int CategoryId);
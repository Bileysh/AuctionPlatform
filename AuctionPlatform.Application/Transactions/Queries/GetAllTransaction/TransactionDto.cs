namespace AuctionPlatform.Application.Transactions.Queries.GetAllTransaction;

public record TransactionDto(
    Guid Id,
    Guid UserId,
    string Username,
    decimal Amount,
    string Type, 
    Guid? ReferenceId,
    DateTime CreatedAt
);
using MediatR;

namespace AuctionPlatform.Application.Transactions.Queries.GetAllTransaction;

public record GetAllTransactionsQuery(): IRequest<List<TransactionDto>>;
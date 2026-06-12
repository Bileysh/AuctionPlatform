using AuctionPlatform.Application.Transactions.Queries.GetAllTransaction;
using MediatR;

namespace AuctionPlatform.Application.Transactions.Queries.GetMyTransactions;

public record GetMyTransactionsQuery() : IRequest<List<TransactionDto>>;
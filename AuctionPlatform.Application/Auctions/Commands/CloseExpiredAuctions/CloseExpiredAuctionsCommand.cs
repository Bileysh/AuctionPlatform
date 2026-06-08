using MediatR;

namespace AuctionPlatform.Application.Auctions.Commands.CloseExpiredAuctions;

public record CloseExpiredAuctionsCommand() : IRequest<int>;
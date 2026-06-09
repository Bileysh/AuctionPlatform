using FluentValidation;

namespace AuctionPlatform.Application.Auctions.Commands.CancelAuction;

public class CancelAuctionCommandValidator: AbstractValidator<CancelAuctionCommand>
{
    public CancelAuctionCommandValidator()
    {
        RuleFor(v => v.Id) 
            .NotEmpty().WithMessage("Auction ID is required.");
    }
}
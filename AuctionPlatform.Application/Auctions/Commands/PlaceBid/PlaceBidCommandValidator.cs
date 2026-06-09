using FluentValidation;

namespace AuctionPlatform.Application.Auctions.Commands.PlaceBid;

public class PlaceBidCommandValidator : AbstractValidator<PlaceBidCommand>
{
    public PlaceBidCommandValidator()
    {
        RuleFor(v => v.AuctionId)
            .NotEmpty().WithMessage("Auction ID is required.");

        RuleFor(v => v.BidderId)
            .NotEmpty().WithMessage("Bidder ID is required.");

        RuleFor(v => v.Amount)
            .GreaterThan(0).WithMessage("Bid amount must be strictly greater than zero!");
    }
}
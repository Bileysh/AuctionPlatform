using FluentValidation;

namespace AuctionPlatform.Application.Auctions.Commands.CreateAuction;

public class CreateAuctionCommandValidator : AbstractValidator<CreateAuctionCommand>
{
    public CreateAuctionCommandValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(v => v.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(v => v.StartingPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Starting price cannot be negative.");

        RuleFor(v => v.EndsAt)
            .GreaterThan(DateTime.UtcNow).WithMessage("Auction end time must be in the future.");

        RuleFor(v => v.CategoryId)
            .GreaterThan(0).WithMessage("Valid Category ID is required.");
    }
}
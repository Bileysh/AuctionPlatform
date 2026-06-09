using FluentValidation;

namespace AuctionPlatform.Application.Auctions.Commands.UpdateAuction;

public class UpdateAuctionCommandValidator: AbstractValidator<UpdateAuctionCommand>
{
    public UpdateAuctionCommandValidator()
    {
       RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(v => v.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");

        RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Id is required.");
        
        RuleFor(v => v.EndsAt)
            .GreaterThan(DateTime.UtcNow).WithMessage("End time must be in the future.");
        
        RuleFor(v => v.CategoryId)
            .NotEmpty().WithMessage("Category ID is required.");
    }
}
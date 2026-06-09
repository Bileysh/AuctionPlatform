using AuctionPlatform.Application.Categories.Commands.CreateCategory;
using FluentValidation;

namespace AuctionPlatform.Application.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandValidator: AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
       RuleFor(v => v.Id)
            .NotEmpty().WithMessage("Category id must be provided!");
    }
}
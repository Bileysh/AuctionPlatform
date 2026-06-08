using MediatR;

namespace AuctionPlatform.Application.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(int Id) : IRequest<bool>;
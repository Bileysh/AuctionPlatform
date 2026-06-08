using MediatR;

namespace AuctionPlatform.Application.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(int Id, string Name): IRequest<bool>;
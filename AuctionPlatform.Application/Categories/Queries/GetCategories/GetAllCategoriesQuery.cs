using MediatR;

namespace AuctionPlatform.Application.Categories.Queries.GetCategories;

public record GetAllCategoriesQuery(): IRequest<List<CategoryDto>>;
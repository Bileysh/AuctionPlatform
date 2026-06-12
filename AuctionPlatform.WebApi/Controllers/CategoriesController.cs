using AuctionPlatform.Application.Categories.Commands.CreateCategory;
using AuctionPlatform.Application.Categories.Commands.DeleteCategory;
using AuctionPlatform.Application.Categories.Commands.UpdateCategory;
using AuctionPlatform.Application.Categories.Queries.GetCategories;
using AuctionPlatform.Application.Categories.Queries.GetCategoriesById;
using AuctionPlatform.WebApi.DTO;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionPlatform.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController: ControllerBase
{
    private readonly ISender _sender;
    
    public CategoriesController(ISender sender)
    {
        _sender = sender;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var categoryId = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(CreateCategory), new { id = categoryId }, categoryId);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken)
    {
        var query = new GetAllCategoriesQuery();
        var categories = await _sender.Send(query, cancellationToken);
        return Ok(categories);
    }
    
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategoryById(int id, CancellationToken cancellationToken)
    {
        var query = new GetCategoriesByIdQuery(id);
        var category = await _sender.Send(query, cancellationToken);
        return Ok(category);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateCategoryCommand(id, request.name);
        var result = await _sender.Send(command, cancellationToken);
        return Ok(new { Success = result, Message = "Category updated successfully" });
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id, CancellationToken cancellationToken)
    {
        var command = new DeleteCategoryCommand(id);
        var result = await _sender.Send(command, cancellationToken);
        return Ok(new { Success = result, Message = "Category deleted successfully" });
    }
}
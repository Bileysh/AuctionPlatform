using AuctionPlatform.Application.Common.Exceptions;
using AuctionPlatform.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace AuctionPlatform.WebApi.Infrastructure;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);
        
        if (exception is NotFoundException notFoundException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            await httpContext.Response.WriteAsJsonAsync(new
            {
                Title = "Not Found",
                Status = 404,
                Detail = notFoundException.Message
            }, cancellationToken);
            
            return true;
        }
        
        if (exception is ValidationException validationException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            
            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key, 
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            await httpContext.Response.WriteAsJsonAsync(new
            {
                Title = "Validation Error",
                Status = 400,
                Detail = "Одне або кілька полів не пройшли перевірку.",
                Errors = errors 
            }, cancellationToken);

            return true;
        }
        
        if (exception is BusinessRuleException businessException)
        {
            _logger.LogWarning("Business rule violation: {Message}", businessException.Message);
          
            httpContext.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
            await httpContext.Response.WriteAsJsonAsync(new
            {
                Title = "Business Rule Violation",
                Status = 422,
                Detail = businessException.Message
            }, cancellationToken);
            
            return true;
        }
        
        if (exception is DbUpdateConcurrencyException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
            await httpContext.Response.WriteAsJsonAsync(new
            {
                Title = "Concurrency Conflict",
                Status = 409,
                Detail = "Дані були змінені іншим користувачем або фоновим процесом. Будь ласка, оновіть сторінку і спробуйте ще раз."
            }, cancellationToken);
            
            return true; 
        }
        
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server Error",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Detail = exception.Message
        };
        
        httpContext.Response.StatusCode = problemDetails.Status.Value;
        
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }
}
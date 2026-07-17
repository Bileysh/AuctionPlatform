using AuctionPlatform.Application.Auctions.Commands.CloseExpiredAuctions;
using MediatR;

namespace AuctionPlatform.WebApi.BackgroundJobs;

public class AuctionClosingWorker: BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<AuctionClosingWorker> _logger;
    
    public AuctionClosingWorker(IServiceScopeFactory scopeFactory, ILogger<AuctionClosingWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Auction Closing Worker started.");

        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(1));
        
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();

                var closedCount = await sender.Send(new CloseExpiredAuctionsCommand(), stoppingToken);
                
                if (closedCount > 0)
                {
                    _logger.LogInformation("Successfully closed {ClosedCount} expired auctions.", closedCount);                
                }
                
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while closing expired auctions.");
                
            }

        }
    }
}
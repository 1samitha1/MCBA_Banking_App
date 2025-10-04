namespace CustomerPortal.Services.Impl;

public class BillPayBackgroundService: BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<BackgroundService> _logger;

    public BillPayBackgroundService(IServiceProvider services, ILogger<BackgroundService> logger)
    {
        _services = services;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // create a scope for hosted service
            using (var scope = _services.CreateScope())
            {
                var billService = scope.ServiceProvider.GetRequiredService<IBillPayService>();
                await billService.ProcessDueBills();
            }

            // run every minute
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); 
            _logger.LogInformation("BillPayBackgroundService ran at {Time}", DateTimeOffset.Now);
        }
    }
}
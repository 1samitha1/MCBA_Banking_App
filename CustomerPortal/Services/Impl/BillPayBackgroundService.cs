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

            // run every 20 seconds
            await Task.Delay(20000, stoppingToken); 
            
            // log the information to the server
            _logger.LogInformation("\n-- BillPayBackgroundService ran at {Time}", DateTimeOffset.Now + " -- \n");
        }
    }
}
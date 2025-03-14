
namespace KutuphaneOtomasyon.Services
{
    public class CezaArkaPlanService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CezaArkaPlanService> _logger;
        private readonly TimeSpan timeSpan = TimeSpan.FromHours(24);
        public CezaArkaPlanService(IServiceScopeFactory scopeFactory, ILogger<CezaArkaPlanService> logger)
        {
            _scopeFactory = scopeFactory;
             _logger = logger;
            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Servis Çalışıyor");
                using (var scope = _scopeFactory.CreateScope())
                {
                    var cezaService = scope.ServiceProvider.GetRequiredService<Cezaservice>();  

                    try
                    {
                        cezaService.CezaUygula(); // Ceza işlemini uygula
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Serviste hata oluştu: {ex.Message}");
                    }
                }
                await Task.Delay(timeSpan, stoppingToken);

            }
        }
    }
}

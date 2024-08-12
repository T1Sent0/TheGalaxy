

using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

using System.Globalization;

using TheGalaxy.Database.Configurate;
using TheGalaxy.Database.Transport;

var host = Host.CreateDefaultBuilder();
var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();

Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");

host.ConfigureServices(serviceCollection =>
{
    ConfigurateService.ConfigureService(serviceCollection, config);
    ConfigugeMassTransit.ConfigureBusControl(serviceCollection, config);
    Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
    serviceCollection.AddLogging(l => l.AddConsole());
});

var build = host.Build();
var busControl = build.Services.GetRequiredService<IBusControl>();
await busControl.StartAsync();

await build.RunAsync();
Console.WriteLine("Service Running.... Press enter to exit");
Console.ReadLine();
busControl.Stop();

internal class SeebDbHostedService : IHostedService, IDisposable
{
    private int executionCount = 0;
    private readonly ILogger<SeebDbHostedService> _logger;
    private Timer? _timer = null;
    private readonly SeedDb _seedDb;

    public SeebDbHostedService(ILogger<SeebDbHostedService> logger, SeedDb seedDb)
    {
        _logger = logger;
        _seedDb = seedDb;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(30));

        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        var count = Interlocked.Increment(ref executionCount);

        _logger.LogInformation(
            "Timed Hosted Service is working. Count: {Count}", count);


        if (count > 1)
        {
            Dispose();
            return;
        }

        Task.Run(async () =>
        {
            await _seedDb.InitRoles();
            await _seedDb.InitUsers();
        });
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
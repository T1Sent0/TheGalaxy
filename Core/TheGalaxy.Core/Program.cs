using TheGalaxy.Core.Transport;

using MassTransit;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

using System.Globalization;

var host = Host.CreateDefaultBuilder();
var config = new ConfigurationBuilder().AddEnvironmentVariables().Build();

Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");

host.ConfigureServices(services =>
{
    ConfigugeMassTransit.RegisterService(services);
    Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
    services.AddLogging(l => l.AddConsole());
});

var build = host.Build();
var busControl = build.Services.GetRequiredService<IBusControl>();
await busControl.StartAsync();

await build.RunAsync();

Console.WriteLine("Service Running.... Press enter to exit");
Console.ReadLine();
busControl.Stop();
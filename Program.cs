using Coravel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureAppConfiguration((hostContext, configApp) =>
    {
        configApp.SetBasePath(Directory.GetCurrentDirectory());
        configApp.AddCommandLine(args);
    })
    .ConfigureServices((hostContext, services) =>
    {
        // Add Coravel's Scheduling...
        services.AddScheduler();
    })
    .Build();

// Configure the scheduled tasks....
host.Services.UseScheduler(scheduler =>
    scheduler.Schedule<MySqlBackupJob>().DailyAtHour(1)
);

host.Run();
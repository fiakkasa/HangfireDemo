using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using HangfireDemo.Attributes;
using HangfireDemo.Interfaces;
using HangfireDemo.Models;
using HangfireDemo.Services;
using Microsoft.Extensions.Options;

namespace HangfireDemo.Extensions;

public static class HangfireExtensions
{
    public static IServiceCollection AddHangfireScheduler(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<HangfireConfig>()
            .BindConfiguration(nameof(HangfireConfig))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<ISchedulerFailedJobHandler, SchedulerFailedJobHandlerService>();

        services.AddHangfire((context, options) =>
        {
            var hangfireConfig = context.GetRequiredService<IOptionsMonitor<HangfireConfig>>().CurrentValue;

            if (hangfireConfig.AutomaticRetryAttempts > 0)
                options.UseFilter(new AutomaticRetryAttribute { Attempts = hangfireConfig.AutomaticRetryAttempts });

            options.UseFilter(new LogOnLastFailureAttribute());

            options.UseMongoStorage(
                configuration.GetConnectionString(hangfireConfig.DatabaseConnectionKey),
                hangfireConfig.DatabaseName,
                new MongoStorageOptions()
                {
                    CheckQueuedJobsStrategy = hangfireConfig.CheckQueuedJobsStrategy,
                    MigrationOptions = new MongoMigrationOptions()
                    {
                        MigrationStrategy = new MigrateMongoMigrationStrategy(),
                        BackupStrategy = new CollectionMongoBackupStrategy()
                    },
                    CheckConnection = hangfireConfig.CheckConnection
                }
            );
        });
        services.AddHangfireServer((serviceProvider, options) =>
        {
            var hangfireConfig = serviceProvider.GetRequiredService<IOptionsMonitor<HangfireConfig>>().CurrentValue;

            options.ServerName = hangfireConfig.ServerName;
        });

        return services;
    }

    public static IApplicationBuilder UseHangfireScheduler(this IApplicationBuilder app)
    {
        var hangfireConfig = app.ApplicationServices.GetRequiredService<IOptionsMonitor<HangfireConfig>>().CurrentValue;
        var shutdownToken = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>().ApplicationStopping;
        var jobManager = app.ApplicationServices.GetRequiredService<IRecurringJobManager>();

        app.UseHangfireDashboard(
            hangfireConfig.DashboardPath,
            new() { DashboardTitle = hangfireConfig.DashboardTitle }
        );

        // recurring job example
        // note: ensure corresponding DI registrations are in place
        jobManager.AddOrUpdate<IProcessText>(
            "Hourly time ticker",
            // note: ensure to pass the application stopping token as to notify hangfire when the application is shutting down and cancel the running job
            job => job.Process($"The time is '{DateTimeOffset.Now:s}'", shutdownToken),
            Cron.Hourly()
        );

        return app;
    }
}

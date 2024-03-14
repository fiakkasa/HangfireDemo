using System.ComponentModel.DataAnnotations;
using Hangfire.Mongo;

namespace HangfireDemo.Models;

public record HangfireConfig
{
    [StringLength(2048, MinimumLength = 1)]
    public string DatabaseConnectionKey { get; init; } = "SchedulerDb";

    [StringLength(128, MinimumLength = 1)]
    public string DatabaseName { get; init; } = "SchedulerDb";

    [StringLength(128, MinimumLength = 1)]
    public string ServerName { get; init; } = "HangfireDemo.Scheduler";

    [Range(0, 128)]
    public int AutomaticRetryAttempts { get; init; } = 3;

    /// <summary>
    /// CheckQueuedJobsStrategy.TailNotificationsCollection, // 📝 single instance
    /// CheckQueuedJobsStrategy.Watch // 📝 replica set
    /// </summary>
    /// <value></value>
    public CheckQueuedJobsStrategy CheckQueuedJobsStrategy { get; init; } = CheckQueuedJobsStrategy.TailNotificationsCollection;

    public bool CheckConnection { get; init; } = true;

    [RegularExpression(@"\/[^\s]+")]
    [StringLength(128, MinimumLength = 1)]
    public string DashboardPath { get; init; } = "/hangfire";

    [StringLength(128, MinimumLength = 1)]
    public string DashboardTitle { get; init; } = "Hangfire Dashboard";
}

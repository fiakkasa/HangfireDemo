using HangfireDemo.Interfaces;

namespace HangfireDemo.Services;

public class SchedulerFailedJobHandlerService(ILogger<SchedulerFailedJobHandlerService> logger) : ISchedulerFailedJobHandler
{
    public Task Handle(string id, string name, string reason, DateTimeOffset timestamp, string serverId, Exception exception)
    {
        logger.LogError(
            exception,
            "Job id '{JobId}' and name '{JobName}' on server with id '{ServerId}' failed at {Timestamp:s} with reason '{FailureReason}'.",
            id,
            name,
            serverId,
            timestamp,
            reason
        );

        return Task.CompletedTask;
    }
}

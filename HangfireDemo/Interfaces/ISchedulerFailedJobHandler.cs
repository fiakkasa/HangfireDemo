namespace HangfireDemo.Interfaces;

public interface ISchedulerFailedJobHandler
{
    Task Handle(string id, string name, string reason, DateTimeOffset timestamp, string serverId, Exception exception);
}

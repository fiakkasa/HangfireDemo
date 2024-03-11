using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;
using HangfireDemo.Interfaces;

namespace HangfireDemo.Attributes;

public class LogOnLastFailureAttribute : JobFilterAttribute, IApplyStateFilter
{
    public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        if (context.NewState is not FailedState failedState) return;

        BackgroundJob.Enqueue<ISchedulerFailedJobHandler>(x =>
            x.Handle(
                context.BackgroundJob.Id,
                context.BackgroundJob.Job.ToString(),
                failedState.Reason,
                failedState.FailedAt,
                failedState.ServerId,
                failedState.Exception
            )
        );
    }

    public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        // skip
    }
}

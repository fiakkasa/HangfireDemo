using Hangfire;
using HangfireDemo.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HangfireDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class ScheduleJobOnDemandController : ControllerBase
{
    [HttpGet]
    public bool Schedule(
        string text,
        [FromServices] IBackgroundJobClient backgroundJobClient,
        [FromServices] ILogger<ScheduleJobOnDemandController> logger,
        [FromServices] IHostApplicationLifetime hostApplicationLifetime
    )
    {
        logger.LogInformation("Job Schedule Requested!");

        // note: ensure to pass the application stopping token as to notify hangfire when the application is shutting down and cancel the running job
        var jobId = backgroundJobClient.Enqueue<IProcessText>(x => x.Process(text, hostApplicationLifetime.ApplicationStopping));

        logger.LogInformation("Job Scheduled with id '{JobId}'!", jobId);

        return true;
    }
}

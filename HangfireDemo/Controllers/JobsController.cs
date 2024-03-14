using System.ComponentModel.DataAnnotations;
using Hangfire;
using HangfireDemo.Interfaces;
using HangfireDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace HangfireDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class JobsController : ControllerBase
{
    [HttpGet]
    [Route("enqueue")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public bool Enqueue(
        [StringLength(256, MinimumLength = 1)] string text,
        [FromServices] IBackgroundJobClient backgroundJobClient,
        [FromServices] ILogger<JobsController> logger,
        [FromServices] IHostApplicationLifetime hostApplicationLifetime
    )
    {
        logger.LogInformation("Job Enqueue Requested!");

        // note: ensure to pass the application stopping token as to notify hangfire when the application is shutting down and cancel the running job
        var jobId = backgroundJobClient.Enqueue<IProcessText>(x => 
            x.Process(text, hostApplicationLifetime.ApplicationStopping)
        );

        logger.LogInformation("Job Enqueued with id '{JobId}'!", jobId);

        return true;
    }

    [HttpPost]
    [Route("schedule")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status500InternalServerError)]
    public bool Schedule(
        ScheduleRequest request,
        [FromServices] IBackgroundJobClient backgroundJobClient,
        [FromServices] ILogger<JobsController> logger,
        [FromServices] IHostApplicationLifetime hostApplicationLifetime
    )
    {
        logger.LogInformation("Job Schedule Requested!");

        // note: ensure to pass the application stopping token as to notify hangfire when the application is shutting down and cancel the running job
        var jobId = backgroundJobClient.Schedule<IProcessText>(x => 
            x.Process(request.Text, hostApplicationLifetime.ApplicationStopping), 
            request.Delay
        );

        logger.LogInformation("Job Scheduled with id '{JobId}'!", jobId);

        return true;
    }
}

using HangfireDemo.Interfaces;

namespace HangfireDemo.Services;

public class ProcessTextService(ILogger<ProcessTextService> logger) : IProcessText
{
    public async Task Process(string text, CancellationToken cancellationToken = default)
    {
        var textSample = text switch
        {
            { Length: > 10 } => text[0..9] + "..",
            _ => text
        };

        logger.LogInformation("Starting to process text '{TextSample}'!", textSample);

        // note: simulate an exception
        if (text == "crash") throw new InvalidOperationException("Splash!");

        // note: simulate work
        await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);

        logger.LogInformation("Finished processing text '{TextSample}'!", textSample);
    }
}

namespace HangfireDemo.Interfaces;

public interface IProcessText
{
    Task Process(string text, CancellationToken cancellationToken = default);
}

using System.ComponentModel.DataAnnotations;

namespace HangfireDemo.Models;

public record ScheduleRequest : IValidatableObject
{
    [StringLength(256, MinimumLength = 1)]
    public string Text { get; init; } = string.Empty;

    public TimeSpan Delay { get; init; } = TimeSpan.FromSeconds(30);

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Delay is not { TotalMilliseconds: > 0, TotalHours: <= 1 })
            yield return new("Value must represent a positive time frame between 1 ms and 1 hour", [nameof(Delay)]);
    }
}
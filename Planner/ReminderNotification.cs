namespace Planner;

public class ReminderNotification
{
    public Guid ItemId { get; init; }
    public required string Title { get; init; }
    public DateTime ItemDateTime { get; init; }
    public DateTime TriggeredAt { get; init; }
    public TimeSpan? ReminderBefore { get; init; }
}
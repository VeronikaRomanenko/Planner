namespace Planner;

public class ReminderService
{
    private readonly PlannerManager _planner;

    public ReminderService(PlannerManager planner)
    {
        _planner = planner;
    }

    public IReadOnlyList<ReminderNotification> GetReminders(DateTime dateTime)
    {
        var notifications = new List<ReminderNotification>();

        foreach (var item in _planner.Items)
        {
            var notification = TryCreateNotification(item, dateTime);
            if (notification != null) notifications.Add(notification);
        }
        
        return notifications;
    }

    public static ReminderNotification? TryCreateNotification(PlannerItem item, DateTime dateTime)
    {
        var itemDateTime = item.GetRelevantDateTime();
        
        if (item.ReminderSent || !itemDateTime.HasValue) return null;
        
        var reminderTime = itemDateTime.Value - item.ReminderBefore;

        if (reminderTime > dateTime || itemDateTime.Value < dateTime) return null;
        
        item.MarkReminderSent();

        return new ReminderNotification
        {
            ItemId = item.Id,
            Title = item.Title,
            ItemDateTime = itemDateTime.Value,
            TriggeredAt = dateTime,
            ReminderBefore = item.ReminderBefore,
        };
    }
}
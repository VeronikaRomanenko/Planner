namespace Planner;

public abstract class PlannerItem
{
    private readonly List<Performer> _performers = [];
    
    public Guid Id { get; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string? Location { get; private set; }
    public TimeSpan ReminderBefore { get; private set; } = TimeSpan.FromMinutes(30);
    public bool ReminderSent { get; private set; }
    
    public IReadOnlyList<Performer> Performers => _performers;
    
    protected PlannerItem(
        string title,
        string description,
        string? location = null,
        TimeSpan? reminderBefore = null
    )
    {
        Id = Guid.NewGuid();

        SetTitle(title);
        SetDescription(description);
        SetLocation(location);
        if (reminderBefore.HasValue) SetReminder(reminderBefore.Value);
    }

    public void SetTitle(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        Title = title;
    }

    public void SetDescription(string description)
    {
        Description = description;
    }

    public void SetLocation(string? location)
    {
        Location = string.IsNullOrWhiteSpace(location) ? null : location;
    }

    public void SetReminder(TimeSpan reminderBefore)
    {
        ReminderBefore = reminderBefore;
        ResetReminder();
    }

    public void MarkReminderSent()
    {
        ReminderSent = true;
    }

    public void ResetReminder()
    {
        ReminderSent = false;
    }

    public void AddPerformer(Performer performer)
    {
        ArgumentNullException.ThrowIfNull(performer);

        if (_performers.Any(p => p.Id == performer.Id))
            return;

        _performers.Add(performer);
    }

    public bool RemovePerformer(Guid performerId)
    {
        var performer = _performers.FirstOrDefault(p => p.Id == performerId);

        if (performer is null)
            return false;

        _performers.Remove(performer);
        return true;
    }

    public abstract void SetRelevantDateTime(DateTime? newDate);
    public abstract DateTime? GetRelevantDateTime();
}
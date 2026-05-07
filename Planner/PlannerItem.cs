namespace Planner;

public abstract class PlannerItem
{
    private readonly List<Performer> _performers = [];
    
    public Guid Id { get; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime? StartTime { get; private set; }
    public string? Location { get; private set; }
    
    public IReadOnlyList<Performer> Performers => _performers;
    
    protected PlannerItem(
        string title,
        string description,
        DateTime? startTime = null,
        string? location = null
    )
    {
        Id = Guid.NewGuid();

        SetTitle(title);
        SetDescription(description);
        SetStartTime(startTime);
        SetLocation(location);
    }

    public void SetTitle(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        Title = title;
    }

    public void SetDescription(string description)
    {
        Description = description ?? string.Empty;
    }
    
    public void SetStartTime(DateTime? startTime)
    {
        StartTime = startTime;
    }

    public void SetLocation(string? location)
    {
        Location = string.IsNullOrWhiteSpace(location) ? null : location;
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

    public abstract DateTime? GetRelevantDateTime();
}
namespace Planner;

public class PlannerEvent : PlannerItem
{
    public TimeSpan Duration { get; private set; }
    
    public DateTime EndTime => StartTime!.Value.Add(Duration);
    
    public PlannerEvent(
        string title,
        string description,
        DateTime startTime,
        TimeSpan duration,
        string location
    ) : base(title, description, startTime, location)
    {
        SetDuration(duration);
    }
    
    public void SetDuration(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
        {
            throw new ArgumentException("Duration must be greater than zero.");
        }
        
        Duration = duration;
    }
    
    public override DateTime? GetRelevantDateTime()
    {
        return StartTime;
    }
}
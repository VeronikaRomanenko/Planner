namespace Planner;

public class PlannerEvent : PlannerItem
{
    public DateTime StartTime { get; private set; }
    public TimeSpan Duration { get; private set; }
    
    public DateTime EndTime => StartTime.Add(Duration);
    
    public PlannerEvent(
        string title,
        string description,
        DateTime startTime,
        TimeSpan duration,
        string location
    ) : base(title, description, location)
    {
        SetStartTime(startTime);
        SetDuration(duration);
    }
    
    public void SetStartTime(DateTime startTime)
    {
        StartTime = startTime;
    }
    
    public void SetDuration(TimeSpan duration)
    {
        if (duration <= TimeSpan.Zero)
        {
            throw new ArgumentException("Duration must be greater than zero.");
        }
        
        Duration = duration;
    }

    public override void SetRelevantDateTime(DateTime? newDate)
    {
        if (!newDate.HasValue)
            throw new ArgumentException(
                "Event must have a start date."
            );
        
        StartTime = newDate.Value;
    }

    public override DateTime? GetRelevantDateTime()
    {
        return StartTime;
    }
}
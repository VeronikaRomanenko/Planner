namespace Planner;

public class PlannerTask : PlannerItem
{
    public DateTime? DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    
    public PlannerTask(
        string title,
        string description,
        DateTime? dueDate = null,
        DateTime? startTime = null,
        string? location = null
    ) : base(title, description, startTime, location)
    {
        SetDueDate(dueDate);
    }

    public void SetDueDate(DateTime? dueDate)
    {
        if (StartTime.HasValue && dueDate.HasValue && StartTime.Value > dueDate.Value)
            throw new ArgumentException("Start time cannot be later than due date.");

        DueDate = dueDate;
    }

    public void Complete(DateTime completedAt)
    {
        IsCompleted = true;
        CompletedAt = completedAt;
    }

    public void Reopen()
    {
        IsCompleted = false;
        CompletedAt = null;
    }

    public bool IsOverdue(DateTime now)
    {
        return !IsCompleted && DueDate.HasValue && DueDate.Value < now;
    }

    public override DateTime? GetRelevantDateTime()
    {
        return StartTime ?? DueDate;
    }
}
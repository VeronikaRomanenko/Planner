namespace Planner;

public class PlannerTask : PlannerItem
{
    public DateTime? DueDate { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    
    public PlannerTask(
        string title,
        string description,
        DateTime? dueDate = null,
        string? location = null
    ) : base(title, description, location)
    {
        SetDueDate(dueDate);
    }

    public void SetDueDate(DateTime? dueDate)
    {
        DueDate = dueDate;
        ResetReminder();
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

    public override void SetRelevantDateTime(DateTime? newDate)
    {
        SetDueDate(newDate);
    }

    public override DateTime? GetRelevantDateTime()
    {
        return DueDate;
    }
}
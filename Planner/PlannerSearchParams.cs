namespace Planner;

public class PlannerSearchParams
{
    public DateTime? StartDate { get; init; }
    public DateTime? EndDate { get; init; }
    public Guid? PerformerId { get; init; }
    public string? Query { get; init; }
    public string? Location { get; init; }
    public bool? IsCompleted { get; init; }
}
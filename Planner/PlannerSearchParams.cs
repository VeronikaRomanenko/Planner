namespace Planner;

public class PlannerSearchParams
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? PerformerId { get; set; }
    public string? Query { get; set; }
    public string? Location { get; set; }
    public bool? IsCompleted { get; set; }
}
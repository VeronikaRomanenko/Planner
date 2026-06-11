namespace Planner;

public class PlannerManager
{
    private readonly List<PlannerItem> _items = [];
    private readonly List<Performer> _performers = [];

    public IReadOnlyList<PlannerItem> Items => _items;
    public IReadOnlyList<Performer> Performers => _performers;

    public void AddItem(PlannerItem item)
    {
        _items.Add(item);
    }

    public PlannerItem? GetItemById(Guid id)
    {
        return _items.FirstOrDefault(i => i.Id == id);
    }

    public void RemoveItem(Guid id)
    {
        _items.RemoveAll(x => x.Id == id);
    }

    public Performer AddPerformer(string name, string email)
    {
        var performer = new Performer(name, email);

        _performers.Add(performer);

        return performer;
    }

    public void RemovePerformer(Guid id)
    {
        var performer = _performers.FirstOrDefault(p => p.Id == id);

        if (performer is null)
            return;

        _performers.Remove(performer);

        foreach (var item in _items)
            item.RemovePerformer(id);
    }

    public Performer? GetPerformerById(Guid id)
    {
        return _performers.FirstOrDefault(p => p.Id == id);
    }

    public IEnumerable<PlannerItem> SearchItems(PlannerSearchParams searchParams)
    {
        return _items.Where(item =>
            (searchParams.StartDate is null || item.GetRelevantDateTime() >= searchParams.StartDate.Value)
            && (searchParams.EndDate is null || item.GetRelevantDateTime() <= searchParams.EndDate.Value)
            && (searchParams.PerformerId is null || item.Performers.Any(p => p.Id == searchParams.PerformerId.Value))
            && (searchParams.Query is null ||
                item.Title.Contains(searchParams.Query, StringComparison.CurrentCultureIgnoreCase) ||
                item.Description.Contains(searchParams.Query, StringComparison.CurrentCultureIgnoreCase))
            && (searchParams.Location is null || (item.Location is not null &&
                                                  item.Location.Contains(searchParams.Location,
                                                      StringComparison.CurrentCultureIgnoreCase)))
            && (searchParams.IsCompleted is null ||
                (item is PlannerTask task && task.IsCompleted == searchParams.IsCompleted)));
    }

    public IEnumerable<PlannerEvent> FindOverlaps(PlannerSearchParams searchParams)
    {
        var events = SearchItems(searchParams).OfType<PlannerEvent>();

        return events
            .Where(e1 => events
                .Any(e2 => e2.Id != e1.Id &&
                           e1.Performers.Any(p => e2.Performers.Any(y => y.Id == p.Id)) &&
                           e1.StartTime < e2.EndTime &&
                           e1.EndTime > e2.StartTime));
    }

    public IEnumerable<PlannerEvent> FindOverlapsForEvent(DateTime start, TimeSpan duration,
        IReadOnlyList<Performer> performers, Guid? id = null)
    {
        var end = start + duration;

        return _items
            .OfType<PlannerEvent>()
            .Where(e =>
                e.Id != id &&
                e.Performers.Any(p => performers.Any(y => y.Id == p.Id)) &&
                e.StartTime < end &&
                e.EndTime > start
            );
    }

    public IEnumerable<PlannerEvent> FindOverlapsForEvent(PlannerEvent item)
    {
        return FindOverlapsForEvent(item.StartTime, item.Duration, item.Performers, item.Id);
    }

    public int MoveUncompletedTasks(DateTime start, DateTime end, DateTime newDate)
    {
        var uncompleted = _items.Where(item =>
            item is PlannerTask task && !task.IsCompleted && task.GetRelevantDateTime() >= start &&
            task.GetRelevantDateTime() <= end).ToList();

        foreach (var task in uncompleted)
        {
            task.SetRelevantDateTime(newDate);
        }

        return uncompleted.Count;
    }

    public int DeleteItemsForPeriod(DateTime start, DateTime end)
    {
        return _items.RemoveAll(item => item.GetRelevantDateTime() >= start && item.GetRelevantDateTime() <= end);
    }
}
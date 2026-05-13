namespace Planner;

public class PlannerManager
{
    private readonly List<PlannerItem> _items = [];
    private readonly List<Performer> _performers = [];

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

    public bool RemovePerformer(Guid id)
    {
        var performer = _performers.FirstOrDefault(p => p.Id == id);

        if (performer is null)
            return false;

        _performers.Remove(performer);

        foreach (var item in _items)
            item.RemovePerformer(id);

        return true;
    }

    public Performer? GetPerformerById(Guid id)
    {
        return _performers.FirstOrDefault(p => p.Id == id);
    }

    public IEnumerable<PlannerItem> SearchItems(PlannerSearchParams searchParams)
    {
        // TODO: filter by all params
        return _items.Where(item =>
            (searchParams.StartDate is null || item.GetRelevantDateTime() >= searchParams.StartDate.Value)
            && (searchParams.EndDate is null || item.GetRelevantDateTime() <= searchParams.EndDate.Value)
            && (searchParams.PerformerId is null || item.Performers.Any(p => p.Id == searchParams.PerformerId.Value))
            && (searchParams.Query is null ||
                item.Title.Contains(searchParams.Query, StringComparison.CurrentCultureIgnoreCase))
            && (searchParams.Location is null || (item.Location is not null &&
                                                  item.Location.Contains(searchParams.Location,
                                                      StringComparison.CurrentCultureIgnoreCase)))
            && (searchParams.IsCompleted is null ||
                (item is PlannerTask task && task.IsCompleted == searchParams.IsCompleted)));
    }

    public IEnumerable<PlannerItem> FindOverlapsForItem(PlannerItem item)
    {
        // TODO: implement
        return _items.Where(x => x.Id != item.Id);
    }

    public IReadOnlyList<PlannerItem> Items => _items;
    public IReadOnlyList<Performer> Performers => _performers;
}
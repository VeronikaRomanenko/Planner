namespace Planner;

public class PlannerManager
{
    private readonly List<PlannerItem> _items = [];

    public void AddItem(PlannerItem item)
    {
        _items.Add(item);
    }

    public void RemoveItem(Guid id)
    {
        _items.RemoveAll(x => x.Id == id);
    }

    public IReadOnlyList<PlannerItem> GetItems(PlannerSearchParams searchParams)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<PlannerItem> Items => _items;
}
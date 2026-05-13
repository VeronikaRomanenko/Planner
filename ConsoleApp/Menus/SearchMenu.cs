using ConsoleApp.Helpers;
using Planner;

namespace ConsoleApp.Menus;

public class SearchMenu
{
    private readonly PlannerManager _planner;

    public SearchMenu(PlannerManager planner)
    {
        _planner = planner;
    }

    public void Run()
    {
        Console.WriteLine();
        Console.WriteLine("=== Пошук ===");

        var query = ConsoleInput.ReadOptionalString("Пошуковий запит (Enter — пропустити): ");
        var start = ConsoleInput.ReadOptionalDateTime("Початок періоду (Enter — пропустити): ");
        var end = ConsoleInput.ReadOptionalDateTime("Кінець періоду (Enter — пропустити): ");
        var location = ConsoleInput.ReadOptionalString("Місце (Enter — пропустити): ");

        var parameters = new PlannerSearchParams()
        {
            Query = query,
            StartDate = start,
            EndDate = end,
            Location = location
        };

        var results = _planner.SearchItems(parameters);

        ConsolePrinter.PrintItems(results);
    }
}
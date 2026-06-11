using ConsoleApp.Helpers;
using Planner;

namespace ConsoleApp.Menus;

public class PeriodViewMenu
{
    private readonly PlannerManager _planner;

    public PeriodViewMenu(PlannerManager planner)
    {
        _planner = planner;
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Перегляд за період ===");
            Console.WriteLine("1. Всі");
            Console.WriteLine("2. Вчора");
            Console.WriteLine("3. Сьогодні");
            Console.WriteLine("4. Завтра");
            Console.WriteLine("5. Післязавтра");
            Console.WriteLine("6. Власний період");
            Console.WriteLine("7. Назад");

            var command = ConsoleInput.ReadRequiredString("Оберіть команду: ");

            if (command == "7")
                return;

            var period = PeriodSelector.Select(command);
            
            if (period is null)
            {
                Console.WriteLine("Невідома команда.");
                continue;
            }
            
            ShowPeriod(period.Value.Start, period.Value.End);
        }
    }
    
    private void ShowPeriod(DateTime? start, DateTime? end)
    {
        var searchParams = new PlannerSearchParams()
        {
            StartDate = start,
            EndDate = end
        };
        
        var items = _planner.SearchItems(searchParams);

        ConsolePrinter.PrintItems(items);

        var overlaps = _planner.FindOverlaps(searchParams);

        if (overlaps.Any())
        {
            ConsolePrinter.PrintOverlaps(overlaps);
        }
        
        new ListActionsMenu(_planner, items.ToList(), start, end).Run();
    }
}
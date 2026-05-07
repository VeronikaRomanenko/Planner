using Planner;
using ConsoleApp.Helpers;

namespace ConsoleApp.Menus;

public class MainMenu
{
    private readonly PlannerManager _planner;
    
    public MainMenu(PlannerManager planner)
    {
        _planner = planner;
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine("=== Електронний щоденник справ ===");
            Console.WriteLine("1. Додати");
            Console.WriteLine("2. Переглянути за період");
            Console.WriteLine("3. Пошук");
            Console.WriteLine("4. Завершити роботу");
            
            var command = ConsoleInput.ReadRequiredString("Оберіть команду: ");
            
            switch (command)
            {
                case "1":
                    new AddItemMenu(_planner).Run();
                    break;

                case "2":
                    new PeriodViewMenu(_planner).Run();
                    break;

                case "3":
                    new SearchMenu(_planner).Run();
                    break;

                case "4":
                    return;

                default:
                    Console.WriteLine("Невідома команда.");
                    break;
            }
        }
    }
}
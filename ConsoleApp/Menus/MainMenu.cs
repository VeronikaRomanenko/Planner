using Planner;
using ConsoleApp.Helpers;

namespace ConsoleApp.Menus;

public class MainMenu
{
    private readonly PlannerManager _planner;
    private readonly NotificationQueue _queue;
    
    public MainMenu(PlannerManager planner, NotificationQueue queue)
    {
        _planner = planner;
        _queue = queue;
    }

    public void Run()
    {
        while (true)
        {
            ConsolePrinter.PrintNotifications(_queue);
            
            Console.WriteLine("=== Електронний щоденник справ ===");
            Console.WriteLine("1. Додати");
            Console.WriteLine("2. Переглянути за період");
            Console.WriteLine("3. Пошук");
            Console.WriteLine("4. Виконавці");
            Console.WriteLine("5. Завершити роботу");
            
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
                    new PerformersMenu(_planner).Run();
                    break;

                case "5":
                    return;

                default:
                    Console.WriteLine("Невідома команда.");
                    break;
            }
        }
    }
}
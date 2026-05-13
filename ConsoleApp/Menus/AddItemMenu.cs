using ConsoleApp.Helpers;
using Planner;

namespace ConsoleApp.Menus;

public class AddItemMenu
{
    private readonly PlannerManager _planner;

    public AddItemMenu(PlannerManager planner)
    {
        _planner = planner;
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Додати запис ===");
            Console.WriteLine("1. Додати справу");
            Console.WriteLine("2. Додати захід");
            Console.WriteLine("3. Назад");

            var command = ConsoleInput.ReadRequiredString("Оберіть команду: ");

            switch (command)
            {
                case "1":
                    AddTask();
                    break;

                case "2":
                    AddEvent();
                    break;

                case "3":
                    return;

                default:
                    Console.WriteLine("Невідома команда.");
                    break;
            }
        }
    }
    
    private void AddEvent()
    {
        var title = ConsoleInput.ReadRequiredString("Назва заходу: ");
        var description = ConsoleInput.ReadOptionalString("Опис: ");
        var startTime = ConsoleInput.ReadDateTime("Дата і час початку: ");
        var duration = ConsoleInput.ReadTimeSpanMinutes("Тривалість у хвилинах: ");
        var location = ConsoleInput.ReadRequiredString("Місце проведення: ");

        var item = new PlannerEvent(title, description, startTime, duration, location);

        if (!ConfirmOverlaps(item))
            return;

        _planner.AddItem(item);
        
        Console.WriteLine("Виконавці: ");
        new ItemPerformersMenu(_planner, item).Run();
        
        Console.WriteLine("Захід додано.");
    }

    private void AddTask()
    {
        var title = ConsoleInput.ReadRequiredString("Назва справи: ");
        var description = ConsoleInput.ReadOptionalString("Опис: ");
        var startTime = ConsoleInput.ReadOptionalDateTime("Дата і час початку (Enter — пропустити): ");
        var dueDate = ConsoleInput.ReadOptionalDateTime("Дедлайн (Enter — пропустити): ");
        var location = ConsoleInput.ReadOptionalString("Місце (Enter — пропустити): ");

        var task = new PlannerTask(title, description, dueDate, startTime, location);

        if (!ConfirmOverlaps(task))
            return;

        _planner.AddItem(task);
        
        Console.WriteLine("Виконавці: ");
        new ItemPerformersMenu(_planner, task).Run();
        
        Console.WriteLine("Справу додано.");
    }

    private bool ConfirmOverlaps(PlannerItem item)
    {
        var overlaps = _planner.FindOverlapsForItem(item);

        if (!overlaps.Any())
            return true;

        Console.WriteLine();
        Console.WriteLine("Увага! Знайдено накладки:");
        // ConsolePrinter.PrintOverlaps(overlaps);

        Console.WriteLine("1. Створити з накладкою");
        Console.WriteLine("2. Скасувати створення");

        var command = ConsoleInput.ReadRequiredString("Оберіть дію: ");
        return command == "1";
    }
}
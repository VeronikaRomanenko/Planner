using ConsoleApp.Helpers;
using Planner;

namespace ConsoleApp.Menus;

public class EditItemMenu
{
    private readonly PlannerManager _planner;
    private readonly PlannerItem _item;

    public EditItemMenu(PlannerManager planner, PlannerItem item)
    {
        _planner = planner;
        _item = item;
    }
    
    public void Run()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine($"=== Редагування: {_item.Title} ===");
            Console.WriteLine("1. Змінити назву");
            Console.WriteLine("2. Змінити опис");
            Console.WriteLine("3. Змінити дату/час початку");
            Console.WriteLine("4. Змінити місце");
            Console.WriteLine("5. Змінити виконавців");

            switch (_item)
            {
                case PlannerEvent:
                    Console.WriteLine("6. Змінити тривалість");
                    Console.WriteLine("7. Назад");
                    break;
                case PlannerTask:
                    Console.WriteLine("6. Змінити дедлайн");
                    Console.WriteLine("7. Позначити як виконану/невиконану");
                    Console.WriteLine("8. Назад");
                    break;
            }

            var command = ConsoleInput.ReadRequiredString("Оберіть дію: ");

            switch (command)
            {
                case "1":
                    _item.SetTitle(ConsoleInput.ReadRequiredString("Нова назва: "));
                    break;

                case "2":
                    _item.SetDescription(ConsoleInput.ReadOptionalString("Новий опис: "));
                    break;

                case "3":
                    ChangeStartTime();
                    break;

                case "4":
                    _item.SetLocation(ConsoleInput.ReadOptionalString("Нове місце: "));
                    break;
                
                case "5":
                    new ItemPerformersMenu(_planner, _item).Run();
                    break;

                case "6" when _item is PlannerEvent eventItem:
                    ChangeEventDuration(eventItem);
                    break;

                case "6" when _item is PlannerTask task:
                    task.SetDueDate(ConsoleInput.ReadOptionalDateTime("Новий дедлайн: "));
                    break;

                case "7" when _item is PlannerTask task:
                    ToggleTaskCompleted(task);
                    break;

                case "7":
                case "8":
                    return;

                default:
                    Console.WriteLine("Невідома команда.");
                    break;
            }
        }
    }

    private void ChangeStartTime()
    {
        var oldStartTime = _item.StartTime;
        var newStartTime = ConsoleInput.ReadOptionalDateTime("Нова дата/час початку: ");

        _item.SetStartTime(newStartTime);

        var overlaps = _planner.FindOverlapsForItem(_item);

        if (!overlaps.Any())
        {
            Console.WriteLine("Дата/час оновлено.");
            return;
        }

        Console.WriteLine("Увага! Після зміни виникають накладки:");
        // ConsolePrinter.PrintOverlaps(overlaps);

        if (!ConsoleInput.Confirm("Залишити зміни?"))
        {
            _item.SetStartTime(oldStartTime);
            Console.WriteLine("Зміни скасовано.");
        }
    }

    private void ChangeEventDuration(PlannerEvent eventItem)
    {
        var oldDuration = eventItem.Duration;
        var newDuration = ConsoleInput.ReadTimeSpanMinutes("Нова тривалість у хвилинах: ");

        eventItem.SetDuration(newDuration);

        var overlaps = _planner.FindOverlapsForItem(eventItem);

        if (!overlaps.Any())
        {
            Console.WriteLine("Тривалість оновлено.");
            return;
        }

        Console.WriteLine("Увага! Після зміни виникають накладки:");
        // ConsolePrinter.PrintOverlaps(overlaps);

        if (!ConsoleInput.Confirm("Залишити зміни?"))
        {
            eventItem.SetDuration(oldDuration);
            Console.WriteLine("Зміни скасовано.");
        }
    }

    private static void ToggleTaskCompleted(PlannerTask task)
    {
        if (task.IsCompleted)
        {
            task.Reopen();
            Console.WriteLine("Справу знову позначено як невиконану.");
        }
        else
        {
            task.Complete(DateTime.Now);
            Console.WriteLine("Справу виконано.");
        }
    }
}
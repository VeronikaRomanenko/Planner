using ConsoleApp.Helpers;
using Planner;

namespace ConsoleApp.Menus;

public class PeriodActionsMenu
{
    private readonly PlannerManager _planner;
    private readonly IEnumerable<PlannerItem> _items;
    private readonly DateTime? _start;
    private readonly DateTime? _end;

    public PeriodActionsMenu(
        PlannerManager planner,
        IEnumerable<PlannerItem> items,
        DateTime? start,
        DateTime? end)
    {
        _planner = planner;
        _items = items;
        _start = start;
        _end = end;
    }
    
    public void Run()
    {
        while (true)
        {
            var isPastPeriod = _end.HasValue && _end.Value < DateTime.Now;

            Console.WriteLine();
            Console.WriteLine("=== Дії зі списком ===");
            Console.WriteLine("1. Редагувати");
            Console.WriteLine("2. Перенести справу");
            Console.WriteLine("3. Виконати справу");
            Console.WriteLine("4. Видалити запис");

            if (isPastPeriod)
            {
                Console.WriteLine("5. Перенести всі невиконані справи");
                Console.WriteLine("6. Видалити всі записи за період");
                Console.WriteLine("7. Назад");
            }
            else
            {
                Console.WriteLine("5. Назад");
            }

            var command = ConsoleInput.ReadRequiredString("Оберіть дію: ");

            switch (command)
            {
                case "1":
                    EditItem();
                    break;

                case "2":
                    MoveTask();
                    break;

                case "3":
                    CompleteTask();
                    break;

                case "4":
                    DeleteItem();
                    break;

                case "5" when isPastPeriod:
                    MoveAllUncompletedTasks();
                    break;

                case "6" when isPastPeriod:
                    DeleteAllPeriodItems();
                    break;

                case "5":
                case "7":
                    return;

                default:
                    Console.WriteLine("Невідома команда.");
                    break;
            }
        }
    }
    
    private PlannerItem? SelectItem()
    {
        if (!_items.Any())
        {
            Console.WriteLine("Список порожній.");
            return null;
        }

        var id = ConsoleInput.ReadGuid("Введіть ID запису: ");
        var item = _planner.GetItemById(id);

        if (item is null)
            Console.WriteLine("Запис не знайдено.");

        return item;
    }

    private void EditItem()
    {
        var item = SelectItem();

        if (item is null)
            return;

        new EditItemMenu(_planner, item).Run();
    }

    private void MoveTask()
    {
        var item = SelectItem();

        if (item is not PlannerTask task)
        {
            Console.WriteLine("Перенесення доступне тільки для справ.");
            return;
        }

        var newDate = ConsoleInput.ReadDateTime("Нова дата/час: ");
        task.SetDueDate(newDate);

        Console.WriteLine("Справу перенесено.");
    }

    private void CompleteTask()
    {
        var item = SelectItem();

        if (item is not PlannerTask task)
        {
            Console.WriteLine("Виконання доступне тільки для справ.");
            return;
        }

        task.Complete(DateTime.Now);
        Console.WriteLine("Справу позначено як виконану.");
    }

    private void DeleteItem()
    {
        var item = SelectItem();

        if (item is null)
            return;

        if (ConsoleInput.Confirm("Видалити запис?"))
        {
            _planner.RemoveItem(item.Id);
            Console.WriteLine("Запис видалено.");
        }
    }

    private void MoveAllUncompletedTasks()
    {
        if (!_start.HasValue || !_end.HasValue)
            return;

        var newDate = ConsoleInput.ReadDateTime("Нова дата для перенесення: ");
        // var count = _planner.MoveUncompletedTasks(_start.Value, _end.Value, newDate);
        //
        // Console.WriteLine($"Перенесено справ: {count}");
    }

    private void DeleteAllPeriodItems()
    {
        if (!_start.HasValue || !_end.HasValue)
            return;

        if (!ConsoleInput.Confirm("Видалити всі записи за вибраний період?"))
            return;

        // var count = _planner.DeleteItemsForPeriod(_start.Value, _end.Value);
        //
        // Console.WriteLine($"Видалено записів: {count}");
    }
}
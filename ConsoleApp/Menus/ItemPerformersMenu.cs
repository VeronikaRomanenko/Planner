using ConsoleApp.Helpers;
using Planner;

namespace ConsoleApp.Menus;

public class ItemPerformersMenu
{
    private readonly PlannerManager _planner;
    private readonly PlannerItem _item;

    public ItemPerformersMenu(PlannerManager planner, PlannerItem item)
    {
        _planner = planner;
        _item = item;
    }

    public void Run()
    {
        while (true)
        {
            if (_item.Performers.Count > 0)
            {
                Console.WriteLine("Поточні виконавці:");
                ConsolePrinter.PrintPerformers(_item.Performers);
                Console.WriteLine();
            }

            // TODO: show only performers that are not yet linked to the item
            if (_planner.Performers.Count > 0)
            {
                Console.WriteLine("Доступні виконавці:");
                ConsolePrinter.PrintPerformers(_planner.Performers);
                Console.WriteLine();
            }
            
            // TODO: change list depending on current and available performers
            Console.WriteLine("1. Додати виконавця із списку");
            Console.WriteLine("2. Видалити виконавця із запису");
            Console.WriteLine("3. Створити нового виконавця");
            Console.WriteLine("4. Назад");
            
            var command = ConsoleInput.ReadRequiredString("Оберіть команду: ");

            switch (command)
            {
                case "1":
                    AddFromList();
                    break;

                case "2":
                    RemovePerformer();
                    break;

                case "3":
                    CreateNew();
                    break;
                
                case "4":
                    return;

                default:
                    Console.WriteLine("Невідома команда.");
                    break;
            }
        }
    }
    
    private Performer? SelectPerformer()
    {
        if (!_planner.Performers.Any())
        {
            Console.WriteLine("Список порожній.");
            return null;
        }

        var index = ConsoleInput.ReadIndex("Введіть номер виконавця: ", _planner.Performers.Count);
        var performer = _planner.Performers[index];

        return performer;
    }

    private void AddFromList()
    {
        // TODO: ask for index, add to item
    }

    private void RemovePerformer()
    {
        // TODO: ask for index, remove from item
    }

    private void CreateNew()
    {
        // TODO: ask for name and email, create in planner, add to item
    }
}
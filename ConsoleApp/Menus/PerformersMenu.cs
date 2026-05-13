using ConsoleApp.Helpers;
using Planner;

namespace ConsoleApp.Menus;

public class PerformersMenu
{
    private readonly PlannerManager _planner;

    public PerformersMenu(PlannerManager planner)
    {
        _planner = planner;
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Виконавці ===");
            ConsolePrinter.PrintPerformers(_planner.Performers);
            Console.WriteLine("1. Додати виконавця");
            Console.WriteLine("2. Редагувати виконавця");
            Console.WriteLine("3. Видалити виконавця");
            Console.WriteLine("4. Назад");

            var command = ConsoleInput.ReadRequiredString("Оберіть команду: ");

            switch (command)
            {
                case "1":
                    AddPerformer();
                    break;

                case "2":
                    EditPerformer();
                    break;
                
                case "3":
                    DeletePerformer();
                    break;
                
                case "4":
                    return;

                default:
                    Console.WriteLine("Невідома команда.");
                    break;
            }
        }
    }
    
    private Performer? SelectItem()
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

    private void AddPerformer()
    {
        var name = ConsoleInput.ReadRequiredString("Імʼя виконавця: ");
        var email = ConsoleInput.ReadOptionalString("Електронна пошта виконавця: ");

        _planner.AddPerformer(name, email);
    }

    private void EditPerformer()
    {
        var performer = SelectItem();
        
        if (performer is null)
            return;

        new EditPerformerMenu(performer).Run();
    }

    private void DeletePerformer()
    {
        var performer = SelectItem();

        if (performer is null)
            return;
        
        if (ConsoleInput.Confirm("Видалити виконавця? Це також видалить його з усіх задач."))
        {
            _planner.RemovePerformer(performer.Id);
            Console.WriteLine("Виконавця видалено.");
        }
    }
}
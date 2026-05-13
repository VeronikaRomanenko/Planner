using ConsoleApp.Helpers;
using Planner;

namespace ConsoleApp.Menus;

public class EditPerformerMenu
{
    private readonly Performer _performer;
    
    public EditPerformerMenu(Performer performer)
    {
        _performer = performer;
    }

    public void Run()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine($"=== Редагування: {_performer.Name} ===");
            Console.WriteLine("1. Змінити імʼя");
            Console.WriteLine("2. Змінити електронну пошту");
            Console.WriteLine("3. Назад");
            
            var command = ConsoleInput.ReadRequiredString("Оберіть дію: ");

            switch (command)
            {
                case "1":
                    _performer.SetName(ConsoleInput.ReadRequiredString("Нове імʼя: "));
                    break;
                
                case "2":
                    _performer.SetEmail(ConsoleInput.ReadOptionalString("Нова електронна пошта: "));
                    break;
                
                case "3":
                    return;
                
                default:
                    Console.WriteLine("Невідома команда.");
                    break;
            }
        }
    }
}
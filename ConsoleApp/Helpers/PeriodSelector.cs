namespace ConsoleApp.Helpers;

public static class PeriodSelector
{
    public static (DateTime? Start, DateTime? End)? Select(string command)
    {
        var today = DateTime.Today;

        return command switch
        {
            "1" => (null, null),

            "2" => (
                today.AddDays(-1),
                today
            ),

            "3" => (
                today,
                today.AddDays(1)
            ),

            "4" => (
                today.AddDays(1),
                today.AddDays(2)
            ),

            "5" => (
                today.AddDays(2),
                today.AddDays(3)
            ),

            "6" => SelectCustomPeriod(),

            _ => null
        };
    }

    private static (DateTime? Start, DateTime? End) SelectCustomPeriod()
    {
        Console.WriteLine();
        Console.WriteLine("=== Власний період ===");

        var start = ConsoleInput.ReadDateTime("Початок періоду: ");
        var end = ConsoleInput.ReadDateTime("Кінець періоду: ");

        while (end <= start)
        {
            Console.WriteLine("Кінець періоду має бути пізніше за початок.");
            end = ConsoleInput.ReadDateTime("Кінець періоду: ");
        }

        return (start, end);
    }

    public static bool IsPastPeriod(DateTime? start, DateTime? end)
    {
        return end.HasValue && end.Value <= DateTime.Now;
    }
}
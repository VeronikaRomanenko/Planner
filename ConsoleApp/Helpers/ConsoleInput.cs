namespace ConsoleApp.Helpers;

public static class ConsoleInput
{
    public static string ReadRequiredString(string message)
    {
        while (true)
        {
            Console.Write(message);

            var value = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(value))
                return value.Trim();

            PrintValidationError("Значення не може бути порожнім.");
        }
    }

    public static string ReadOptionalString(string message)
    {
        Console.Write(message);

        var value = Console.ReadLine();

        return string.IsNullOrWhiteSpace(value)
            ? string.Empty
            : value.Trim();
    }

    public static DateTime ReadDateTime(string message)
    {
        while (true)
        {
            Console.Write(message);

            var value = Console.ReadLine();

            if (DateTime.TryParse(value, out var dateTime))
                return dateTime;

            PrintValidationError(
                "Невірний формат дати. Приклад: 10.05.2026 14:30"
            );
        }
    }

    public static DateTime? ReadOptionalDateTime(string message)
    {
        while (true)
        {
            Console.Write(message);

            var value = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (DateTime.TryParse(value, out var dateTime))
                return dateTime;

            PrintValidationError(
                "Невірний формат дати. Приклад: 10.05.2026 14:30"
            );
        }
    }

    public static TimeSpan ReadTimeSpanMinutes(string message)
    {
        while (true)
        {
            Console.Write(message);

            var value = Console.ReadLine();

            if (int.TryParse(value, out var minutes) && minutes > 0)
                return TimeSpan.FromMinutes(minutes);

            PrintValidationError(
                "Введіть додатне число хвилин."
            );
        }
    }

    public static int ReadPositiveInt(string message)
    {
        while (true)
        {
            Console.Write(message);

            var value = Console.ReadLine();

            if (int.TryParse(value, out var number) && number > 0)
                return number;

            PrintValidationError(
                "Введіть додатне ціле число."
            );
        }
    }

    public static Guid ReadGuid(string message)
    {
        while (true)
        {
            Console.Write(message);

            var value = Console.ReadLine();

            if (Guid.TryParse(value, out var guid))
                return guid;

            PrintValidationError(
                "Невірний формат GUID."
            );
        }
    }

    public static bool Confirm(string message)
    {
        while (true)
        {
            Console.Write($"{message} (y/n): ");

            var value = Console.ReadLine()?.Trim().ToLower();

            switch (value)
            {
                case "y":
                case "yes":
                case "т":
                case "так":
                    return true;

                case "n":
                case "no":
                case "н":
                case "ні":
                    return false;

                default:
                    PrintValidationError(
                        "Введіть y/n."
                    );
                    break;
            }
        }
    }

    private static void PrintValidationError(string message)
    {
        var oldColor = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ForegroundColor = oldColor;
    }
}
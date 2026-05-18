using Planner;

namespace ConsoleApp.Helpers;

public static class ConsolePrinter
{
    private static string BuildRow(IReadOnlyList<string> cells, IReadOnlyList<int> widths)
    {
        var values = cells.Select((cell, index) => $" {cell.PadRight(widths[index])} ");

        return "|" + string.Join("|", values) + "|";
    }
    
    public static void PrintTable(
        IReadOnlyList<string> headers,
        IReadOnlyList<IReadOnlyList<string>> rows,
        string? notFoundMessage = null
    )
    {
        if (!rows.Any() && notFoundMessage != null)
        {
            Console.WriteLine(notFoundMessage);
            return;
        }

        var widths = headers
            .Select((header, index) =>
                Math.Max(
                    header.Length,
                    rows.Max(row => row[index].Length)
                )
            )
            .ToArray();

        var separator = "+" + string.Join("+", widths.Select(width => new string('-', width + 2))) + "+";

        Console.WriteLine(separator);
        Console.WriteLine(BuildRow(headers, widths));
        Console.WriteLine(separator);

        foreach (var row in rows)
            Console.WriteLine(BuildRow(row, widths));

        Console.WriteLine(separator);
    }
    
    public static void PrintItems(IEnumerable<PlannerItem> items)
    {
        var itemsList = items.ToList();

        if (!itemsList.Any())
        {
            Console.WriteLine("Записи не знайдено.");
            return;
        }

        var rows = itemsList.Select((item, index) => new[]
        {
            (index + 1).ToString(),
            item switch
            {
                PlannerEvent => "Захід",
                PlannerTask => "Справа",
                _ => "Запис"
            },
            item.Title,
            item switch
            {
                PlannerEvent eventItem =>
                    $"{eventItem.StartTime:dd.MM.yyyy HH:mm} - {eventItem.EndTime:dd.MM.yyyy HH:mm}",

                PlannerTask { DueDate: not null } taskItem =>
                    $"{taskItem.DueDate.Value:dd.MM.yyyy HH:mm}",

                _ => "-"
            },
            string.IsNullOrWhiteSpace(item.Location) ? "-" : item.Location,
            item.Performers.Any()
                ? string.Join(", ", item.Performers.Select(p => p.Name))
                : "-",
            item is PlannerTask task
                ? task.IsCompleted ? "Виконано" : "Не виконано"
                : "-"
        }).ToList<IReadOnlyList<string>>();

        PrintTable(
            ["№", "Тип", "Назва", "Дата/час", "Місце", "Виконавці", "Статус"],
            rows
        );

        Console.WriteLine($"Всього записів: {itemsList.Count}");
    }

    public static void PrintPerformers(IEnumerable<Performer> performers)
    {
        var performersList = performers.ToList();
        
        if (!performersList.Any())
        {
            Console.WriteLine("Виконавців ще немає");
            return;
        }
        
        var rows = performersList.Select((item, index) => new[]
        {
            (index + 1).ToString(),
            item.Name,
            string.IsNullOrWhiteSpace(item.Email) ? "-" : item.Email
        }).ToList<IReadOnlyList<string>>();
        
        PrintTable(
            ["№", "Імʼя", "Електронна пошта"],
            rows
        );
    }
    
    public static void PrintOverlaps(
        IEnumerable<PlannerEvent> overlaps
    )
    {
        var overlapsList = overlaps.ToList();
    
        if (overlapsList.Count == 0)
        {
            Console.WriteLine("Накладки не знайдено.");
            return;
        }
        
        foreach (var overlap in overlapsList)
        {
            Console.WriteLine($"{overlap.Title}: {overlap.StartTime:dd.MM.yyyy HH:mm} - {overlap.EndTime:dd.MM.yyyy HH:mm}");
        }
    }

    // public static void PrintOverlaps(
    //     IEnumerable<(PlannerItem First, PlannerItem Second)> overlaps
    // )
    // {
    //     var overlapsList = overlaps.ToList();
    //
    //     if (!overlapsList.Any())
    //     {
    //         Console.WriteLine("Накладки не знайдено.");
    //         return;
    //     }
    //
    //     foreach (var overlap in overlapsList)
    //     {
    //         Console.ForegroundColor = ConsoleColor.Yellow;
    //
    //         Console.WriteLine(
    //             $"- \"{overlap.First.Title}\" перетинається з \"{overlap.Second.Title}\""
    //         );
    //
    //         Console.ResetColor();
    //
    //         PrintOverlapItemInfo(overlap.First);
    //         PrintOverlapItemInfo(overlap.Second);
    //
    //         Console.WriteLine();
    //     }
    // }
    //
    // private static void PrintOverlapItemInfo(PlannerItem item)
    // {
    //     switch (item)
    //     {
    //         case PlannerEvent eventItem:
    //             Console.WriteLine(
    //                 $"  {eventItem.StartTime:dd.MM.yyyy HH:mm} - {eventItem.EndTime:HH:mm}"
    //             );
    //             break;
    //
    //         case PlannerTask taskItem
    //             when taskItem.StartTime.HasValue &&
    //                  taskItem.EstimatedDuration.HasValue:
    //
    //             var end = taskItem.StartTime.Value
    //                 .Add(taskItem.EstimatedDuration.Value);
    //
    //             Console.WriteLine(
    //                 $"  {taskItem.StartTime:dd.MM.yyyy HH:mm} - {end:HH:mm}"
    //             );
    //
    //             break;
    //     }
    // }

    // public static void PrintNotifications(
    //     Services.NotificationQueue queue
    // )
    // {
    //     var notifications = queue.DequeueAll();
    //
    //     foreach (var notification in notifications)
    //     {
    //         Console.ForegroundColor = ConsoleColor.Magenta;
    //
    //         Console.WriteLine();
    //         Console.WriteLine(
    //             $"[НАГАДУВАННЯ] {notification}"
    //         );
    //
    //         Console.ResetColor();
    //     }
    // }
}
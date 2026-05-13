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
            item.StartTime?.ToString("dd.MM.yyyy HH:mm") ?? "-",
            GetEndDate(item),
            string.IsNullOrWhiteSpace(item.Location) ? "-" : item.Location,
            item.Performers.Any()
                ? string.Join(", ", item.Performers.Select(p => p.Name))
                : "-",
            item is PlannerTask task
                ? task.IsCompleted ? "Виконано" : "Не виконано"
                : "-"
        }).ToList<IReadOnlyList<string>>();

        // TODO: change end date to end/due date
        PrintTable(
            ["№", "Тип", "Назва", "Початок", "Кінець", "Місце", "Виконавці", "Статус"],
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

    // TODO: move somewhere
    private static string GetEndDate(PlannerItem item)
    {
        return item switch
        {
            PlannerEvent eventItem =>
                eventItem.EndTime.ToString("dd.MM.yyyy HH:mm"),

            PlannerTask taskItem when taskItem.DueDate.HasValue =>
                taskItem.DueDate.Value.ToString("dd.MM.yyyy HH:mm"),

            PlannerTask taskItem
                when taskItem.StartTime.HasValue && taskItem.EstimatedDuration.HasValue =>
                taskItem.StartTime.Value
                    .Add(taskItem.EstimatedDuration.Value)
                    .ToString("dd.MM.yyyy HH:mm"),

            _ => "-"
        };
    }

    public static void PrintItem(PlannerItem item)
    {
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Cyan;

        Console.WriteLine(
            item switch
            {
                PlannerEvent => "[ЗАХІД]",
                PlannerTask => "[СПРАВА]",
                _ => "[ЗАПИС]"
            }
        );

        Console.ResetColor();

        Console.WriteLine($"ID: {item.Id}");
        Console.WriteLine($"Назва: {item.Title}");

        if (!string.IsNullOrWhiteSpace(item.Description))
            Console.WriteLine($"Опис: {item.Description}");

        if (item.StartTime.HasValue)
        {
            Console.WriteLine(
                $"Початок: {item.StartTime.Value:dd.MM.yyyy HH:mm}"
            );
        }

        if (!string.IsNullOrWhiteSpace(item.Location))
            Console.WriteLine($"Місце: {item.Location}");

        if (item.Performers.Any())
        {
            Console.WriteLine(
                $"Виконавці: {string.Join(", ", item.Performers.Select(p => p.Name))}"
            );
        }

        switch (item)
        {
            case PlannerEvent eventItem:
                PrintEvent(eventItem);
                break;

            case PlannerTask taskItem:
                PrintTask(taskItem);
                break;
        }
    }

    private static void PrintEvent(PlannerEvent item)
    {
        Console.WriteLine(
            $"Тривалість: {item.Duration.TotalMinutes} хв."
        );

        Console.WriteLine(
            $"Кінець: {item.EndTime:dd.MM.yyyy HH:mm}"
        );
    }

    private static void PrintTask(PlannerTask item)
    {
        if (item.DueDate.HasValue)
        {
            Console.WriteLine(
                $"Дедлайн: {item.DueDate.Value:dd.MM.yyyy HH:mm}"
            );
        }

        Console.WriteLine(
            $"Статус: {(item.IsCompleted ? "Виконано" : "Не виконано")}"
        );

        if (item.CompletedAt.HasValue)
        {
            Console.WriteLine(
                $"Виконано: {item.CompletedAt.Value:dd.MM.yyyy HH:mm}"
            );
        }

        if (item.EstimatedDuration.HasValue)
        {
            Console.WriteLine(
                $"Орієнтовна тривалість: {item.EstimatedDuration.Value.TotalMinutes} хв."
            );
        }

        if (!item.IsCompleted && item.IsOverdue(DateTime.Now))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("Увага: справа прострочена.");

            Console.ResetColor();
        }
    }

    public static void PrintOverlaps(
        IEnumerable<(PlannerItem First, PlannerItem Second)> overlaps
    )
    {
        var overlapsList = overlaps.ToList();

        if (!overlapsList.Any())
        {
            Console.WriteLine("Накладки не знайдено.");
            return;
        }

        foreach (var overlap in overlapsList)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine(
                $"- \"{overlap.First.Title}\" перетинається з \"{overlap.Second.Title}\""
            );

            Console.ResetColor();

            PrintOverlapItemInfo(overlap.First);
            PrintOverlapItemInfo(overlap.Second);

            Console.WriteLine();
        }
    }

    private static void PrintOverlapItemInfo(PlannerItem item)
    {
        switch (item)
        {
            case PlannerEvent eventItem:
                Console.WriteLine(
                    $"  {eventItem.StartTime:dd.MM.yyyy HH:mm} - {eventItem.EndTime:HH:mm}"
                );
                break;

            case PlannerTask taskItem
                when taskItem.StartTime.HasValue &&
                     taskItem.EstimatedDuration.HasValue:

                var end = taskItem.StartTime.Value
                    .Add(taskItem.EstimatedDuration.Value);

                Console.WriteLine(
                    $"  {taskItem.StartTime:dd.MM.yyyy HH:mm} - {end:HH:mm}"
                );

                break;
        }
    }

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
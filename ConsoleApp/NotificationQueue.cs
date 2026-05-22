using System.Collections.Concurrent;

namespace ConsoleApp;

public class NotificationQueue
{
    private readonly ConcurrentQueue<string> _queue = new();

    public void Enqueue(string notification)
    {
        _queue.Enqueue(notification);
    }

    public IReadOnlyList<string> DequeueAll()
    {
        var result = new List<string>();

        while (_queue.TryDequeue(out var notification))
        {
            result.Add(notification);
        }
        
        return result;
    }
}
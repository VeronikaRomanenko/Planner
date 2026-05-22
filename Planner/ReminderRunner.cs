namespace Planner;

public class ReminderRunner: IDisposable
{
    private readonly ReminderService _reminderService;
    private readonly TimeSpan _interval;
    
    private Timer? _timer;
    private bool _isRunning;

    public event Action<ReminderNotification>? ReminderTriggered;

    public ReminderRunner(ReminderService reminderService, TimeSpan interval)
    {
        _reminderService = reminderService;
        _interval = interval;
    }
    
    private void CheckReminders(object? state)
    {
        var notifications = _reminderService.GetReminders(DateTime.Now);
        
        foreach (var notification in notifications) ReminderTriggered?.Invoke(notification);
    }
    
    public void Start()
    {
        if (_isRunning) return;
        
        _timer = new Timer(CheckReminders, null, TimeSpan.Zero, _interval);

        _isRunning = true;
    }

    public void Stop()
    {
        _timer?.Dispose();
        _timer = null;
        _isRunning = false;
    }

    public void Dispose()
    {
        Stop();
    }
}
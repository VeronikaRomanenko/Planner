using Planner;
using ConsoleApp;
using ConsoleApp.Menus;

var planner = new PlannerManager();

DemoData.Seed(planner);

var notificationQueue = new NotificationQueue();
var reminderService = new ReminderService(planner);

using var reminderRunner = new ReminderRunner(reminderService, TimeSpan.FromSeconds(60));

reminderRunner.ReminderTriggered += notification =>
    notificationQueue.Enqueue($"\"{notification.Title}\" — {notification.ItemDateTime:dd.MM.yyyy HH:mm}");

reminderRunner.Start();

var menu = new MainMenu(planner, notificationQueue);
menu.Run();

reminderRunner.Stop();
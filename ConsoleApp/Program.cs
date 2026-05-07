using Planner;
using ConsoleApp;
using ConsoleApp.Menus;

var planner = new PlannerManager();

DemoData.Seed(planner);

var menu = new MainMenu(planner);
menu.Run();
using Planner;

namespace ConsoleApp;

public static class DemoData
{
    public static void Seed(PlannerManager planner)
    {
        var performer1 = new Performer(
            "Вероніка Романенко",
            "veronika@example.com"
        );

        var performer2 = new Performer(
            "Петро Петренко",
            "oleksandr@example.com"
        );

        var event1 = new PlannerEvent(
            "Зустріч з викладачем",
            "Обговорення курсової роботи",
            DateTime.Now.AddHours(2),
            TimeSpan.FromMinutes(60),
            "Аудиторія 204"
        );

        event1.AddPerformer(performer1);

        var event2 = new PlannerEvent(
            "Онлайн консультація",
            "Консультація з UML-діаграм",
            DateTime.Now.AddHours(2).AddMinutes(30),
            TimeSpan.FromMinutes(45),
            "Zoom"
        );

        event2.AddPerformer(performer1);
        event2.AddPerformer(performer2);

        var event3 = new PlannerEvent(
            "Нарада команди",
            "Обговорення архітектури проєкту",
            DateTime.Now.AddHours(3),
            TimeSpan.FromMinutes(90),
            "Teams"
        );

        event3.AddPerformer(performer2);

        var event4 = new PlannerEvent(
            "Презентація курсової",
            "Попередній захист",
            DateTime.Today.AddDays(1).AddHours(14),
            TimeSpan.FromMinutes(30),
            "Аудиторія 310"
        );

        event4.AddPerformer(performer1);

        var event5 = new PlannerEvent(
            "Практична робота",
            "Виконання лабораторної",
            DateTime.Today.AddDays(2).AddHours(10),
            TimeSpan.FromMinutes(120),
            "Комп'ютерний клас"
        );

        event5.AddPerformer(performer2);

        var task1 = new PlannerTask(
            "Підготувати UML-діаграму",
            "Завершити діаграму класів",
            DateTime.Today.AddHours(18),
            DateTime.Today.AddHours(15),
            null
        );

        task1.AddPerformer(performer1);

        var task2 = new PlannerTask(
            "Написати пояснювальну записку",
            "Розділ про ООП-модель",
            DateTime.Today.AddDays(1).AddHours(20),
            DateTime.Today.AddDays(1).AddHours(16),
            "Дім"
        );

        task2.AddPerformer(performer1);

        var task3 = new PlannerTask(
            "Підготувати список літератури",
            "Оформити джерела за ДСТУ",
            DateTime.Today.AddDays(-1).AddHours(18),
            DateTime.Today.AddDays(-1).AddHours(14),
            null
        );

        task3.AddPerformer(performer2);

        var task4 = new PlannerTask(
            "Надіслати звіт викладачу",
            "Надіслати проміжну версію",
            DateTime.Today.AddDays(-1).AddHours(12),
            DateTime.Today.AddDays(-1).AddHours(10),
            null
        );

        task4.AddPerformer(performer1);
        task4.Complete(DateTime.Today.AddDays(-1).AddHours(11));

        var task5 = new PlannerTask(
            "Придбати канцтовари",
            "Купити блокнот і маркери",
            null,
            null,
            "Магазин"
        );

        task5.AddPerformer(performer1);

        var task6 = new PlannerTask(
            "Підготувати презентацію",
            "Створити слайди для захисту",
            DateTime.Now.AddHours(2).AddMinutes(15),
            DateTime.Now.AddHours(2),
            "Дім"
        );

        task6.AddPerformer(performer1);

        planner.AddItem(event1);
        planner.AddItem(event2);
        planner.AddItem(event3);
        planner.AddItem(event4);
        planner.AddItem(event5);

        planner.AddItem(task1);
        planner.AddItem(task2);
        planner.AddItem(task3);
        planner.AddItem(task4);
        planner.AddItem(task5);
        planner.AddItem(task6);
    }
}
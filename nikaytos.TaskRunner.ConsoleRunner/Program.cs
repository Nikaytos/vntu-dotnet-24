using nikaytos.TaskRunner.Domain.Logic;
using nikaytos.TaskRunner.Domain.Models.Enums;
using nikaytos.TaskRunner.Domain.Models;

internal static class Program
{
    public static void Main(string[] args)
    {
        List<WorkItem> workItems = new List<WorkItem>();

        Console.WriteLine("Enter the number of tasks you want to create:");
        int itemCount = int.Parse(Console.ReadLine());

        for (int i = 0; i < itemCount; i++)
        {
            Console.WriteLine($"Enter data for the task {i + 1}");

            Console.Write("Task name: ");
            string title = Console.ReadLine();

            Console.Write("Task description: ");
            string description = Console.ReadLine();

            Console.Write("Completion date (dd.MM.yyyy): ");
            DateTime dueDate = DateTime.Parse(Console.ReadLine());

            Console.Write("Priority (None, Low, Medium, High, Urgent): ");
            Priority priority = Enum.Parse<Priority>(Console.ReadLine(), ignoreCase: true);

            Console.Write("Complexity (None, Minutes, Hours, Days, Weeks): ");
            Complexity complexity = Enum.Parse<Complexity>(Console.ReadLine(), ignoreCase: true);

            workItems.Add(new WorkItem
            {
                Title = title,
                Description = description,
                DueDate = dueDate,
                CreationDate = DateTime.Now,
                Priority = priority,
                Complexity = complexity,
                IsCompleted = false
            });
        }

        SimpleTaskPlanner taskPlanner = new SimpleTaskPlanner();
        WorkItem[] sortedItems = taskPlanner.CreatePlan(workItems.ToArray());

        Console.WriteLine("\nSorted tasks:");
        foreach (var workItem in sortedItems)
        {
            Console.WriteLine(workItem);
        }
    }
}
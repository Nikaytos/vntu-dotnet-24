using nikaytos.TaskRunner.Domain.Logic;
using nikaytos.TaskRunner.Domain.Models.Enums;
using nikaytos.TaskRunner.Domain.Models;
using nikaytos.TaskPlanner.DataAccess.Abstractions;
using nikaytos.TaskPlanner.DataAccess;

internal static class Program
{
    private static readonly IWorkItemsRepository Repository = new FileWorkItemsRepository();
    private static readonly SimpleTaskPlanner TaskPlanner = new SimpleTaskPlanner(Repository);

    public static void Main(string[] args)
    {
        Console.WriteLine("Task Planner Console App");

        while (true)
        {
            Console.WriteLine("Choose an operation:");
            Console.WriteLine("[A]dd work item");
            Console.WriteLine("[B]uild a plan");
            Console.WriteLine("[M]ark work item as completed");
            Console.WriteLine("[R]emove a work item");
            Console.WriteLine("[Q]uit the app");

            var choice = Console.ReadLine()?.ToUpper();
            switch (choice)
            {
                case "A":
                    AddWorkItem();
                    break;
                case "B":
                    BuildPlan();
                    break;
                case "M":
                    MarkCompleted();
                    break;
                case "R":
                    RemoveWorkItem();
                    break;
                case "Q":
                    Console.WriteLine("Quitting the app.");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void AddWorkItem()
    {
        Console.WriteLine("Enter work item details:");

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

        var newWorkItem = new WorkItem
        {
            Title = title,
            Description = description,
            DueDate = dueDate,
            CreationDate = DateTime.Now,
            Priority = priority,
            Complexity = complexity,
            IsCompleted = false
        };

        var id = Repository.Add(newWorkItem);
        Console.WriteLine($"Work item added with ID: {id}");
    }

    private static void BuildPlan()
    {
        var plan = TaskPlanner.CreatePlan();
        if (plan.Length == 0)
        {
            Console.WriteLine("There are no tasks to complete.");
        }
        else
        {
            Console.WriteLine("Work Plan:");
            foreach (var item in plan)
            {
                Console.WriteLine(item);
            }
        }
    }

    private static void MarkCompleted()
    {
        Console.WriteLine("Enter the ID of the work item to mark as completed:");
        if (Guid.TryParse(Console.ReadLine(), out var id))
        {
            var workItem = Repository.Get(id);
            if (workItem != null)
            {
                workItem.IsCompleted = true;
                Repository.Update(workItem);
                Console.WriteLine("Work item marked as completed.");
            }
            else
            {
                Console.WriteLine("Work item not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID format.");
        }
    }

    private static void RemoveWorkItem()
    {
        Console.WriteLine("Enter the ID of the work item to remove:");
        if (Guid.TryParse(Console.ReadLine(), out var id))
        {
            if (Repository.Remove(id))
            {
                Console.WriteLine("Work item removed successfully.");
            }
            else
            {
                Console.WriteLine("Work item not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid ID format.");
        }
    }
}
using nikaytos.TaskRunner.Domain.Models;

namespace nikaytos.TaskRunner.Domain.Logic
{
    public class SimpleTaskPlanner
    {
        public WorkItem[] CreatePlan(WorkItem[] items)
        {
            var itemsAsList = items.ToList();

            itemsAsList.Sort(CompareWorkItems);

            return itemsAsList.ToArray();
        }

        private static int CompareWorkItems(WorkItem firstItem, WorkItem secondItem)
        {
            if (firstItem.Priority != secondItem.Priority)
                return secondItem.Priority.CompareTo(firstItem.Priority);
            if (firstItem.DueDate != secondItem.DueDate)
                return firstItem.DueDate.CompareTo(secondItem.DueDate);
            return string.Compare(firstItem.Title, secondItem.Title, StringComparison.Ordinal);
        }

    }
}
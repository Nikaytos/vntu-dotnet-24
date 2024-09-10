    using nikaytos.TaskPlanner.DataAccess.Abstractions;
    using nikaytos.TaskRunner.Domain.Models;

    namespace nikaytos.TaskRunner.Domain.Logic
    {
        public class SimpleTaskPlanner
        {
            private readonly IWorkItemsRepository _repository;

            public SimpleTaskPlanner(IWorkItemsRepository repository)
            {
                _repository = repository;
            }

            public WorkItem[] CreatePlan()
            {
                var items = _repository.GetAll().Where(item => !item.IsCompleted).ToList();
                items.Sort(CompareWorkItems);
                return items.ToArray();
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
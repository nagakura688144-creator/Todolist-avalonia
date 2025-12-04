// Services/DueDateDescendingSortStrategy.cs
using System.Collections.Generic;
using System.Linq;
using TodoApp.Models;

namespace TodoApp.Services
{
    /// <summary>
    /// Sorts TodoItems by due date in descending order.
    /// Items without due dates are placed at the end.
    /// Items with the same due date are sorted by title.
    /// </summary>
    public class DueDateDescendingSortStrategy : ISortStrategy
    {
        public string Name => "Due Date (Latest First)";

        public IEnumerable<TodoItem> Sort(IEnumerable<TodoItem> items)
        {
            var itemsList = items.ToList();

            // Separate items with and without due dates
            var withDueDate = itemsList
                .Where(x => x.DueDate.HasValue)
                .OrderByDescending(x => x.DueDate!.Value)
                .ThenBy(x => x.Title);

            var withoutDueDate = itemsList
                .Where(x => !x.DueDate.HasValue)
                .OrderBy(x => x.Title);

            // Concatenate: items with due dates first (descending), then items without
            return withDueDate.Concat(withoutDueDate);
        }
    }
}

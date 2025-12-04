// Services/TodoSortService.cs
using System.Collections.Generic;
using TodoApp.Models;

namespace TodoApp.Services
{
    /// <summary>
    /// Service for sorting TodoItems using various strategies.
    /// Follows SOLID principles:
    /// - Single Responsibility: Only handles sorting logic
    /// - Dependency Inversion: Depends on ISortStrategy abstraction
    /// - Open/Closed: Can accept new strategies without modification
    /// </summary>
    public class TodoSortService
    {
        private ISortStrategy _currentStrategy;

        public TodoSortService(ISortStrategy defaultStrategy)
        {
            _currentStrategy = defaultStrategy;
        }

        /// <summary>
        /// Gets or sets the current sorting strategy.
        /// </summary>
        public ISortStrategy CurrentStrategy
        {
            get => _currentStrategy;
            set => _currentStrategy = value;
        }

        /// <summary>
        /// Sorts the given items using the current strategy.
        /// </summary>
        public IEnumerable<TodoItem> Sort(IEnumerable<TodoItem> items)
        {
            return _currentStrategy.Sort(items);
        }

        /// <summary>
        /// Sorts the given items using a specific strategy.
        /// </summary>
        public IEnumerable<TodoItem> Sort(IEnumerable<TodoItem> items, ISortStrategy strategy)
        {
            return strategy.Sort(items);
        }
    }
}

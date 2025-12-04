// Services/ISortStrategy.cs
using System.Collections.Generic;
using TodoApp.Models;

namespace TodoApp.Services
{
    /// <summary>
    /// Strategy interface for sorting TodoItems.
    /// Implements the Strategy Pattern.
    /// Follows SOLID principles:
    /// - Open/Closed: Open for extension (new strategies), closed for modification
    /// - Interface Segregation: Single, focused interface for sorting
    /// </summary>
    public interface ISortStrategy
    {
        /// <summary>
        /// Sorts a list of TodoItems according to the strategy's logic.
        /// </summary>
        /// <param name="items">The list of items to sort</param>
        /// <returns>A new sorted list</returns>
        IEnumerable<TodoItem> Sort(IEnumerable<TodoItem> items);

        /// <summary>
        /// Gets the display name of this sort strategy.
        /// </summary>
        string Name { get; }
    }
}

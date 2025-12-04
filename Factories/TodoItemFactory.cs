// Factories/TodoItemFactory.cs
using System;
using TodoApp.Models;

namespace TodoApp.Factories
{
    /// <summary>
    /// Concrete implementation of ITodoItemFactory.
    /// Provides consistent TodoItem creation logic.
    /// </summary>
    public class TodoItemFactory : ITodoItemFactory
    {
        /// <summary>
        /// Creates a new TodoItem with specified parameters.
        /// Ensures all TodoItems are created with valid IDs and proper initialization.
        /// </summary>
        public TodoItem Create(string title, DateTimeOffset? dueDate = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be null or empty", nameof(title));
            }

            return new TodoItem
            {
                Id = Guid.NewGuid(),
                Title = title.Trim(),
                DueDate = dueDate,
                IsCompleted = false
            };
        }

        /// <summary>
        /// Creates a TodoItem with default placeholder values.
        /// Useful for testing or placeholder scenarios.
        /// </summary>
        public TodoItem CreateWithDefaults()
        {
            return new TodoItem
            {
                Id = Guid.NewGuid(),
                Title = "New Task",
                DueDate = null,
                IsCompleted = false
            };
        }
    }
}

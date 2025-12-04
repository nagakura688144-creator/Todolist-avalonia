// Factories/ITodoItemFactory.cs
using System;
using TodoApp.Models;

namespace TodoApp.Factories
{
    /// <summary>
    /// Factory interface for creating TodoItem instances.
    /// Implements the Factory Pattern (Creational Design Pattern).
    /// Follows SOLID principles:
    /// - Single Responsibility: Only responsible for object creation
    /// - Dependency Inversion: Depend on abstraction, not concrete implementation
    /// </summary>
    public interface ITodoItemFactory
    {
        /// <summary>
        /// Creates a new TodoItem with specified title and optional due date.
        /// </summary>
        /// <param name="title">The title of the todo item</param>
        /// <param name="dueDate">Optional due date</param>
        /// <returns>A new TodoItem instance</returns>
        TodoItem Create(string title, DateTimeOffset? dueDate = null);

        /// <summary>
        /// Creates a new TodoItem with default values.
        /// </summary>
        /// <returns>A new TodoItem with default settings</returns>
        TodoItem CreateWithDefaults();
    }
}

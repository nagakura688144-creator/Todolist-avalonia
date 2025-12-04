// Services/TodoValidationService.cs
using System;

namespace TodoApp.Services
{
    /// <summary>
    /// Service for validating TodoItem data.
    /// Follows SOLID principles:
    /// - Single Responsibility: Only handles validation logic
    /// - Open/Closed: Can be extended with new validation rules
    /// </summary>
    public class TodoValidationService
    {
        /// <summary>
        /// Validates a todo title.
        /// </summary>
        /// <param name="title">The title to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        public bool IsValidTitle(string? title)
        {
            return !string.IsNullOrWhiteSpace(title);
        }

        /// <summary>
        /// Validates a todo title and throws an exception if invalid.
        /// </summary>
        /// <param name="title">The title to validate</param>
        /// <exception cref="ArgumentException">Thrown when title is invalid</exception>
        public void ValidateTitle(string? title)
        {
            if (!IsValidTitle(title))
            {
                throw new ArgumentException("Title cannot be null or empty", nameof(title));
            }
        }

        /// <summary>
        /// Validates a due date.
        /// </summary>
        /// <param name="dueDate">The due date to validate</param>
        /// <returns>True if valid (null is considered valid), false otherwise</returns>
        public bool IsValidDueDate(DateTimeOffset? dueDate)
        {
            // Null is valid (no due date)
            // Any non-null date is valid (even past dates)
            return true;
        }

        /// <summary>
        /// Gets a sanitized (trimmed) version of the title.
        /// </summary>
        public string SanitizeTitle(string? title)
        {
            return title?.Trim() ?? string.Empty;
        }
    }
}

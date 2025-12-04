// TodoApp.Tests/Services/SortStrategyTests.cs
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Tests.Services
{
    /// <summary>
    /// Unit tests for Sort Strategies.
    /// Tests the Strategy Pattern implementation and sorting logic.
    /// </summary>
    public class SortStrategyTests
    {
        [Fact]
        public void DueDateAscendingSortStrategy_ShouldPlaceNullDueDatesLast()
        {
            // Arrange
            var strategy = new DueDateAscendingSortStrategy();
            var items = new List<TodoItem>
            {
                new TodoItem { Title = "Task No Date", DueDate = null },
                new TodoItem { Title = "Task With Date", DueDate = DateTimeOffset.Now.AddDays(1) },
                new TodoItem { Title = "Task No Date 2", DueDate = null }
            };

            // Act
            var sorted = strategy.Sort(items).ToList();

            // Assert
            Assert.Equal("Task With Date", sorted[0].Title);
            Assert.Null(sorted[1].DueDate);
            Assert.Null(sorted[2].DueDate);
        }

        [Fact]
        public void DueDateDescendingSortStrategy_ShouldPlaceNullDueDatesLast()
        {
            // Arrange
            var strategy = new DueDateDescendingSortStrategy();
            var items = new List<TodoItem>
            {
                new TodoItem { Title = "Task No Date", DueDate = null },
                new TodoItem { Title = "Task Late", DueDate = DateTimeOffset.Now.AddDays(3) },
                new TodoItem { Title = "Task Early", DueDate = DateTimeOffset.Now.AddDays(1) }
            };

            // Act
            var sorted = strategy.Sort(items).ToList();

            // Assert
            Assert.Equal("Task Late", sorted[0].Title);
            Assert.Equal("Task Early", sorted[1].Title);
            Assert.Equal("Task No Date", sorted[2].Title);
            Assert.Null(sorted[2].DueDate);
        }
    }
}

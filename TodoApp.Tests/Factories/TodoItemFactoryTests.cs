// TodoApp.Tests/Factories/TodoItemFactoryTests.cs
using System;
using Xunit;
using TodoApp.Factories;
using TodoApp.Models;

namespace TodoApp.Tests.Factories
{
    /// <summary>
    /// Unit tests for TodoItemFactory.
    /// Tests the Factory Pattern implementation.
    /// </summary>
    public class TodoItemFactoryTests
    {
        private readonly TodoItemFactory _factory;

        public TodoItemFactoryTests()
        {
            _factory = new TodoItemFactory();
        }

        [Fact]
        public void Create_ShouldReturnValidTodoItem()
        {
            // Arrange
            var title = "Test Task";
            var dueDate = DateTimeOffset.Now.AddDays(1);

            // Act
            var item = _factory.Create(title, dueDate);

            // Assert
            Assert.NotNull(item);
            Assert.Equal(title, item.Title);
            Assert.Equal(dueDate, item.DueDate);
            Assert.False(item.IsCompleted);
            Assert.NotEqual(Guid.Empty, item.Id);
        }

        [Fact]
        public void Create_WithoutDueDate_ShouldReturnItemWithNullDueDate()
        {
            // Arrange
            var title = "Test Task Without Date";

            // Act
            var item = _factory.Create(title);

            // Assert
            Assert.NotNull(item);
            Assert.Equal(title, item.Title);
            Assert.Null(item.DueDate);
            Assert.False(item.IsCompleted);
            Assert.NotEqual(Guid.Empty, item.Id);
        }

        [Fact]
        public void CreateWithDefaults_ShouldReturnValidTodoItem()
        {
            // Act
            var item = _factory.CreateWithDefaults();

            // Assert
            Assert.NotNull(item);
            Assert.Equal("New Task", item.Title);
            Assert.Null(item.DueDate);
            Assert.False(item.IsCompleted);
            Assert.NotEqual(Guid.Empty, item.Id);
        }
    }
}

// TodoApp.Tests/Controllers/TodoControllerTests.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TodoApp.Controllers;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Tests.Controllers
{
    /// <summary>
    /// Unit tests for TodoController.
    /// Tests CRUD operations and storage interaction.
    /// </summary>
    public class TodoControllerTests
    {
        private class InMemoryTodoStorage : ITodoStorage
        {
            private List<TodoItem> _items = new();

            public List<TodoItem> Load() => _items.ToList();

            public void Save(List<TodoItem> items)
            {
                _items = items.ToList();
            }
        }

        [Fact]
        public async Task AddAsync_ShouldAddItemAndSave()
        {
            // Arrange
            var storage = new InMemoryTodoStorage();
            var controller = new TodoController(storage);
            var currentList = new List<TodoItem>();
            var newItem = new TodoItem { Title = "Test Task", IsCompleted = false };

            // Act
            await controller.AddAsync(currentList, newItem);

            // Assert
            Assert.Contains(newItem, currentList);
            var savedItems = storage.Load();
            Assert.Contains(newItem, savedItems);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateItemAndSave()
        {
            // Arrange
            var storage = new InMemoryTodoStorage();
            var controller = new TodoController(storage);
            var item = new TodoItem { Title = "Original Title", IsCompleted = false };
            var currentList = new List<TodoItem> { item };
            storage.Save(currentList);

            // Act
            item.Title = "Updated Title";
            item.IsCompleted = true;
            await controller.UpdateAsync(currentList, item);

            // Assert
            var savedItems = storage.Load();
            var savedItem = savedItems.First();
            Assert.Equal("Updated Title", savedItem.Title);
            Assert.True(savedItem.IsCompleted);
        }
    }
}

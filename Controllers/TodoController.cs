// Controllers/TodoController.cs
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.Controllers
{
    public sealed class TodoController
    {
        private readonly ITodoStorage _storage;

        public TodoController(ITodoStorage storage)
        {
            _storage = storage;
        }

        public Task<List<TodoItem>> LoadAsync()
            => Task.FromResult(_storage.Load());

        public Task SaveAsync(List<TodoItem> items)
        {
            _storage.Save(items);
            return Task.CompletedTask;
        }

        public Task AddAsync(List<TodoItem> current, TodoItem item)
        {
            current.Add(item);
            _storage.Save(current);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(List<TodoItem> current, TodoItem item)
        {
            current.Remove(item);
            _storage.Save(current);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(List<TodoItem> current, TodoItem _)
        {
            // current は参照更新済み想定
            _storage.Save(current);
            return Task.CompletedTask;
        }
    }
}

// Services/ITodoStorage.cs
using System.Collections.Generic;
using TodoApp.Models;

namespace TodoApp.Services
{
    public interface ITodoStorage
    {
        List<TodoItem> Load();
        void Save(List<TodoItem> items);
    }

    // 保存先のパスを表示するため用
    public interface IStoragePathProvider
    {
        string Path { get; }
    }
}

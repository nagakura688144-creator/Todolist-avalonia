using System.Collections.Generic;
using System.Threading.Tasks;
// ★ namespace はあなたのプロジェクトに合わせる（例: TodolistAvalonia.Services）
namespace TodolistAvalonia.Services
{
    using TodolistAvalonia.Models;

    public interface ITodoStorage
    {
        Task<List<TodoItem>> LoadAsync();
        Task SaveAsync(IEnumerable<TodoItem> items);
    }

    // UIに保存先パスを表示したい場合に使う（任意）
    public interface IStoragePathProvider
    {
        string Path { get; }
    }
}

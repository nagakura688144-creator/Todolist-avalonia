// Services/JsonTodoStorage.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TodoApp.Models;

namespace TodoApp.Services
{
    /// <summary>
    /// JSON ファイルに ToDo を保存/読み込みする実装
    /// </summary>
    public class JsonTodoStorage : ITodoStorage, IStoragePathProvider
    {
        private readonly string _dir;
        private readonly string _file;

        // IStoragePathProvider 実装（保存先のファイル・パスを公開）
        public string Path => _file;

        public JsonTodoStorage()
        {
            // OS に依存しないユーザーデータ保存場所
            var baseDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // ★ ここは必ず System.IO.Path.Combine を使う
            _dir  = System.IO.Path.Combine(baseDir, "TodoApp");
            _file = System.IO.Path.Combine(_dir, "todos.json");

            Directory.CreateDirectory(_dir);
            if (!File.Exists(_file))
            {
                File.WriteAllText(_file, "[]");
            }
        }

        // ---- 同期 API（ITodoStorage が同期用ならこちらが使われます）----
        public List<TodoItem> Load()
        {
            try
            {
                if (!File.Exists(_file)) return new List<TodoItem>();
                var json = File.ReadAllText(_file);
                return JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
            }
            catch
            {
                return new List<TodoItem>();
            }
        }

        public void Save(List<TodoItem> items)
        {
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_file, json);
        }

        // ---- 非同期ヘルパ（コントローラが Async を呼ぶ場合に対応）----
        public Task<List<TodoItem>> LoadAsync() => Task.FromResult(Load());
        public Task SaveAsync(List<TodoItem> items)
        {
            Save(items);
            return Task.CompletedTask;
        }
    }
}

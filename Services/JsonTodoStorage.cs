using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
// ★ namespace はあなたのプロジェクトに合わせる（例: TodolistAvalonia.Services）
namespace TodolistAvalonia.Services
{
    using TodolistAvalonia.Models;

    /// <summary>
    /// ローカルJSONでToDoを保存/読込する実装
    /// </summary>
    public class JsonTodoStorage : ITodoStorage, IStoragePathProvider
    {
        public string Path { get; }

        public JsonTodoStorage(string appName = "TodoApp")
        {
            var baseDir = GetBaseConfigDir();
            var appDir = System.IO.Path.Combine(baseDir, appName);
            Directory.CreateDirectory(appDir);
            Path = System.IO.Path.Combine(appDir, "todos.json");
        }

        public async Task<List<TodoItem>> LoadAsync()
        {
            if (!File.Exists(Path)) return new List<TodoItem>();
            await using var fs = File.OpenRead(Path);
            var list = await JsonSerializer.DeserializeAsync<List<TodoItem>>(fs, JsonOptions);
            return list ?? new List<TodoItem>();
        }

        public async Task SaveAsync(IEnumerable<TodoItem> items)
        {
            // 安全に書き込み（一時ファイル→置き換え）
            var tmp = Path + ".tmp";
            await using (var fs = File.Create(tmp))
            {
                await JsonSerializer.SerializeAsync(fs, items, JsonOptionsIndented);
            }
            if (File.Exists(Path)) File.Delete(Path);
            File.Move(tmp, Path);
        }

        // OSごとの「設定/データ保存」ディレクトリ
        private static string GetBaseConfigDir()
        {
            if (OperatingSystem.IsWindows())
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData); // %APPDATA%
            }
            if (OperatingSystem.IsMacOS())
            {
                // ~/Library/Application Support
                var lib = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                // 一部 .NET 実装で ApplicationData が ~/Library/Application Support を返すため、そのまま利用
                return lib;
            }
            // Linux: ~/.config
            var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return System.IO.Path.Combine(home, ".config");
        }

        // JSON オプション
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private static readonly JsonSerializerOptions JsonOptionsIndented = new()
        {
            WriteIndented = true
        };
    }
}

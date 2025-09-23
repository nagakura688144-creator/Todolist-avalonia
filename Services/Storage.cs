using System.Text.Json;
using TodoApp.Models;

namespace TodoApp.Services;

public static class Storage
{
    private static string GetDataPath()
    {
        var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "TodoApp");
        Directory.CreateDirectory(dir);
        return Path.Combine(dir, "todos.json");
    }

    public static async Task SaveAsync(IEnumerable<TodoItem> items)
    {
        var path = GetDataPath();
        await using var fs = File.Create(path);
        var options = new JsonSerializerOptions { WriteIndented = true };
        await JsonSerializer.SerializeAsync(fs, items, options);
    }

    public static async Task<List<TodoItem>> LoadAsync()
    {
        var path = GetDataPath();
        if (!File.Exists(path)) return new List<TodoItem>();
        await using var fs = File.OpenRead(path);
        var data = await JsonSerializer.DeserializeAsync<List<TodoItem>>(fs);
        return data ?? new List<TodoItem>();
    }
}
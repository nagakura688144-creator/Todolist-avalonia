using ReactiveUI;

namespace TodoApp.Models;

public class TodoItem : ReactiveObject
{
    private string _title = string.Empty;
    private bool _isDone;
    private DateTimeOffset? _due;

    public string Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    public bool IsDone
    {
        get => _isDone;
        set => this.RaiseAndSetIfChanged(ref _isDone, value);
    }

    public DateTimeOffset? Due
    {
        get => _due;
        set => this.RaiseAndSetIfChanged(ref _due, value);
    }
}
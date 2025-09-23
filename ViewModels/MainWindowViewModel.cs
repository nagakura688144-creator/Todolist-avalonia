using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public enum FilterMode { All, Active, Completed }

    public ObservableCollection<TodoItem> Items { get; } = new();
    public ObservableCollection<TodoItem> VisibleItems { get; } = new();
    public Array FilterModes => Enum.GetValues(typeof(FilterMode));

    private FilterMode _filter = FilterMode.All;
    public FilterMode Filter
    {
        get => _filter;
        set
        {
            this.RaiseAndSetIfChanged(ref _filter, value);
            RefreshVisible();
        }
    }

    private TodoItem? _selectedItem;
    public TodoItem? SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

    private string _newTitle = string.Empty;
    public string NewTitle
    {
        get => _newTitle;
        set => this.RaiseAndSetIfChanged(ref _newTitle, value);
    }

    private DateTimeOffset? _newDue;
    public DateTimeOffset? NewDue
    {
        get => _newDue;
        set => this.RaiseAndSetIfChanged(ref _newDue, value);
    }

    public string Summary => $"{Items.Count(i => !i.IsDone)} remaining of {Items.Count}";

    public ReactiveCommand<Unit, Unit> AddCommand { get; }
    public ReactiveCommand<Unit, Unit> DeleteSelectedCommand { get; }
    public ReactiveCommand<Unit, Unit> ClearCompletedCommand { get; }
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> LoadCommand { get; }

    public MainWindowViewModel()
    {
        Items.Add(new TodoItem { Title = "Welcome to Avalonia" });
        Items.Add(new TodoItem { Title = "Build your ToDo app" });
        Items.Add(new TodoItem { Title = "Check 'Completed' filter", IsDone = true });
        HookCollection(Items);
        RefreshVisible();

        var canAdd = this
            .WhenAnyValue(x => x.NewTitle)
            .Select(t => !string.IsNullOrWhiteSpace(t))
            .ObserveOn(RxApp.MainThreadScheduler);
        AddCommand = ReactiveCommand.Create(Add, canAdd);

        var canDelete = this
            .WhenAnyValue(x => x.SelectedItem)
            .Select(si => si != null)
            .ObserveOn(RxApp.MainThreadScheduler);
        DeleteSelectedCommand = ReactiveCommand.Create(DeleteSelected, canDelete);

        ClearCompletedCommand = ReactiveCommand.Create(ClearCompleted);
        SaveCommand = ReactiveCommand.CreateFromTask(SaveAsync);
        LoadCommand = ReactiveCommand.CreateFromTask(LoadAsync);
    }

    private void Add()
    {
        var item = new TodoItem { Title = NewTitle.Trim(), Due = NewDue };
        Items.Add(item);
        NewTitle = string.Empty;
        NewDue = null;
        RefreshVisible();
    }

    private void DeleteSelected()
    {
        if (SelectedItem is null) return;
        Items.Remove(SelectedItem);
        SelectedItem = null;
        RefreshVisible();
    }

    private void ClearCompleted()
    {
        var done = Items.Where(i => i.IsDone).ToList();
        foreach (var i in done) Items.Remove(i);
        RefreshVisible();
    }

    private async Task SaveAsync() => await Storage.SaveAsync(Items);

    private async Task LoadAsync()
    {
        var loaded = await Storage.LoadAsync();
        UnhookCollection(Items);
        Items.Clear();
        foreach (var i in loaded) Items.Add(i);
        HookCollection(Items);
        RefreshVisible();
    }

    private void HookCollection(ObservableCollection<TodoItem> col)
    {
        col.CollectionChanged += OnCollectionChanged;
        foreach (var i in col) i.PropertyChanged += OnItemPropertyChanged;
    }

    private void UnhookCollection(ObservableCollection<TodoItem> col)
    {
        col.CollectionChanged -= OnCollectionChanged;
        foreach (var i in col) i.PropertyChanged -= OnItemPropertyChanged;
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
            foreach (TodoItem i in e.OldItems) i.PropertyChanged -= OnItemPropertyChanged;

        if (e.NewItems != null)
            foreach (TodoItem i in e.NewItems) i.PropertyChanged += OnItemPropertyChanged;

        RefreshVisible();
    }

    private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(TodoItem.IsDone) || e.PropertyName == nameof(TodoItem.Title))
        {
            RefreshVisible();
        }
    }

    private void RefreshVisible()
    {
        VisibleItems.Clear();
        IEnumerable<TodoItem> src = Filter switch
        {
            FilterMode.Active => Items.Where(i => !i.IsDone),
            FilterMode.Completed => Items.Where(i => i.IsDone),
            _ => Items
        };

        foreach (var i in src) VisibleItems.Add(i);
        this.RaisePropertyChanged(nameof(Summary));
    }
}
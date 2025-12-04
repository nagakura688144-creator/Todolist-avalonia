// ViewModels/MainWindowViewModel.cs
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using TodoApp.Controllers;
using TodoApp.Models;
using TodoApp.Services;
using TodoApp.Factories;

namespace TodoApp.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly TodoController _controller;
        private readonly ITodoStorage _storage;
        private readonly ITodoItemFactory _factory;
        private readonly TodoSortService _sortService;
        private readonly TodoValidationService _validationService;
        private bool _isSortAscending = true;
        
        // Reference to main window for dialog display
        public Avalonia.Controls.Window? OwnerWindow { get; set; }

        public MainWindowViewModel(
            TodoController controller, 
            ITodoStorage storage,
            ITodoItemFactory factory,
            TodoSortService sortService,
            TodoValidationService validationService)
        {
            _controller = controller;
            _storage = storage;
            _factory = factory;
            _sortService = sortService;
            _validationService = validationService;

            AddCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var title = _validationService.SanitizeTitle(NewTitle);
                if (!_validationService.IsValidTitle(title)) return;

                var item = _factory.Create(title, NewDueDate);
                var list = Items.ToList();
                await _controller.AddAsync(list, item);

                Items.Add(item);
                NewTitle = "";
                NewDueDate = null;

                ApplySorting();
                RefreshView();
                TouchSaved();
            });

            ClearInputCommand = ReactiveCommand.Create(() =>
            {
                NewTitle = "";
                NewDueDate = null;
            });

            DeleteCommand = ReactiveCommand.CreateFromTask<TodoItem>(async item =>
            {
                var list = Items.ToList();
                await _controller.DeleteAsync(list, item);
                Items.Remove(item);
                RefreshView();
                TouchSaved();
            });

            ToggleCompletedCommand = ReactiveCommand.CreateFromTask<TodoItem>(async item =>
            {
                // TwoWay バインドにより IsCompleted は最新
                var list = Items.ToList();
                await _controller.UpdateAsync(list, item);
                RefreshView();
                TouchSaved();
            });

            ToggleSortCommand = ReactiveCommand.Create(() =>
            {
                _isSortAscending = !_isSortAscending;
                ApplySorting();
                RefreshView();
                this.RaisePropertyChanged(nameof(SortButtonText));
            });
        }

        // ====== データ ======
        public ObservableCollection<TodoItem> Items { get; } = new();

        public System.Collections.Generic.IEnumerable<TodoItem> ViewItems =>
            IsFilterActive ? Items.Where(i => !i.IsCompleted) :
            IsFilterCompleted ? Items.Where(i => i.IsCompleted) : Items;

        private string? _newTitle;
        public string? NewTitle { get => _newTitle; set => this.RaiseAndSetIfChanged(ref _newTitle, value); }

        private DateTimeOffset? _newDue;
        public DateTimeOffset? NewDueDate { get => _newDue; set => this.RaiseAndSetIfChanged(ref _newDue, value); }

        // ====== フィルタ ======
        private bool _fa = true, _fA, _fC;

        public bool IsFilterAll
        {
            get => _fa;
            set
            {
                if (this.RaiseAndSetIfChanged(ref _fa, value))
                {
                    if (value)
                    {
                        _fA = _fC = false;
                        this.RaisePropertyChanged(nameof(IsFilterActive));
                        this.RaisePropertyChanged(nameof(IsFilterCompleted));
                    }
                    RefreshView();
                }
            }
        }

        public bool IsFilterActive
        {
            get => _fA;
            set
            {
                if (this.RaiseAndSetIfChanged(ref _fA, value))
                {
                    if (value)
                    {
                        _fa = _fC = false;
                        this.RaisePropertyChanged(nameof(IsFilterAll));
                        this.RaisePropertyChanged(nameof(IsFilterCompleted));
                    }
                    RefreshView();
                }
            }
        }

        public bool IsFilterCompleted
        {
            get => _fC;
            set
            {
                if (this.RaiseAndSetIfChanged(ref _fC, value))
                {
                    if (value)
                    {
                        _fa = _fA = false;
                        this.RaisePropertyChanged(nameof(IsFilterAll));
                        this.RaisePropertyChanged(nameof(IsFilterActive));
                    }
                    RefreshView();
                }
            }
        }

        // ====== 表示用 ======
        private DateTime _lastSaved = DateTime.Now;
        public string ItemsCountText => $"{Items.Count} tasks";
        public string StatusText => $"Saved {_lastSaved:t}";
        public string StoragePath => (_storage as IStoragePathProvider)?.Path ?? "(local)";

        private void RefreshView()
        {
            this.RaisePropertyChanged(nameof(ViewItems));
            this.RaisePropertyChanged(nameof(ItemsCountText));
        }
        private void TouchSaved() => this.RaisePropertyChanged(nameof(StatusText));

        // ====== ロード ======
        public async Task LoadAsync()
        {
            var loaded = await _controller.LoadAsync();

            // 初回データが空ならサンプル投入
            if (loaded.Count == 0)
            {
                loaded.Add(new TodoItem { Title = "Homework Math", DueDate = new DateTime(DateTime.Now.Year, 12, 15), IsCompleted = false });
                loaded.Add(new TodoItem { Title = "Quiz",           DueDate = new DateTime(DateTime.Now.Year, 10, 10), IsCompleted = false });
                loaded.Add(new TodoItem { Title = "Work",           DueDate = DateTime.Now.Date.AddDays(-3),            IsCompleted = false });
                loaded.Add(new TodoItem { Title = "Homework 2",     DueDate = DateTime.Now.Date.AddDays(-1),            IsCompleted = true  });
                await _controller.SaveAsync(loaded);
            }

            Items.Clear();
            foreach (var it in loaded) Items.Add(it);
            RefreshView();
        }

        // ====== コマンド ======
        public ReactiveCommand<Unit, Unit> AddCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearInputCommand { get; }
        public ReactiveCommand<TodoItem, Unit> DeleteCommand { get; }
        public ReactiveCommand<TodoItem, Unit> ToggleCompletedCommand { get; }
        public ReactiveCommand<Unit, Unit> ToggleSortCommand { get; }

        // ====== ソート関連 ======
        public string SortButtonText => _isSortAscending ? "↑ Earliest First" : "↓ Latest First";

        private void ApplySorting()
        {
            var strategy = _isSortAscending 
                ? (ISortStrategy)new DueDateAscendingSortStrategy() 
                : new DueDateDescendingSortStrategy();
            
            var sorted = _sortService.Sort(Items, strategy).ToList();
            
            Items.Clear();
            foreach (var item in sorted)
            {
                Items.Add(item);
            }
        }


    }
}

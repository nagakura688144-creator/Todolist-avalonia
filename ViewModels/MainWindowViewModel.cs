using Avalonia;
using Avalonia.Styling;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using TodoApp.Controllers;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly TodoController _controller;
        private readonly ITodoStorage _storage;

        // ====== コレクション & フィルタ ======
        public ObservableCollection<TodoItem> Items { get; } = new();

        public ReadOnlyObservableCollection<TodoItem> ViewItems
            => new ReadOnlyObservableCollection<TodoItem>(new ObservableCollection<TodoItem>(
                IsFilterActive ? Items.Where(i => !i.IsCompleted) :
                IsFilterCompleted ? Items.Where(i => i.IsCompleted) :
                Items));

        private void RefreshView()
        {
            // ViewItems は都度生成しているので Items が変わったら通知だけ送り直す
            this.RaisePropertyChanged(nameof(ViewItems));
            this.RaisePropertyChanged(nameof(ItemsCountText));
        }

        // ====== 入力エリア ======
        private string? _newTitle;
        public string? NewTitle
        {
            get => _newTitle;
            set => this.RaiseAndSetIfChanged(ref _newTitle, value);
        }

        private DateTimeOffset? _newDueDate;
        public DateTimeOffset? NewDueDate
        {
            get => _newDueDate;
            set => this.RaiseAndSetIfChanged(ref _newDueDate, value);
        }

        // ====== フィルタ（セグメント） ======
        private bool _isFilterAll = true;
        public bool IsFilterAll
        {
            get => _isFilterAll;
            set
            {
                if (this.RaiseAndSetIfChanged(ref _isFilterAll, value) && value)
                {
                    IsFilterActive = false; IsFilterCompleted = false;
                    RefreshView();
                }
            }
        }

        private bool _isFilterActive;
        public bool IsFilterActive
        {
            get => _isFilterActive;
            set
            {
                if (this.RaiseAndSetIfChanged(ref _isFilterActive, value) && value)
                {
                    IsFilterAll = false; IsFilterCompleted = false;
                    RefreshView();
                }
            }
        }

        private bool _isFilterCompleted;
        public bool IsFilterCompleted
        {
            get => _isFilterCompleted;
            set
            {
                if (this.RaiseAndSetIfChanged(ref _isFilterCompleted, value) && value)
                {
                    IsFilterAll = false; IsFilterActive = false;
                    RefreshView();
                }
            }
        }

        // ====== テーマ切替 ======
        private bool _isDarkTheme;
        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set
            {
                if (this.RaiseAndSetIfChanged(ref _isDarkTheme, value))
                {
                    (Application.Current as Application)!.RequestedThemeVariant =
                        value ? ThemeVariant.Dark : ThemeVariant.Light;
                }
            }
        }

        // ====== ステータス表示 ======
        private DateTime _lastSaved = DateTime.Now;
        public string ItemsCountText => $"{Items.Count} tasks";
        public string StatusText => $"Saved {_lastSaved:t}";
        public string StoragePath { get; }

        private void TouchSaved()
        {
            _lastSaved = DateTime.Now;
            this.RaisePropertyChanged(nameof(StatusText));
        }

        // ====== コマンド ======
        public ReactiveCommand<Unit, Unit> AddCommand { get; }
        public ReactiveCommand<Unit, Unit> ClearInputCommand { get; }
        public ReactiveCommand<TodoItem, Unit> DeleteCommand { get; }
        public ReactiveCommand<TodoItem, Unit> EditCommand { get; } // 簡易: タイトルを再入力するなど
        public ReactiveCommand<TodoItem, Unit> ToggleCompletedCommand { get; }

        // 並べ替え (DnD 用)
        public async Task ReorderAsync(int oldIndex, int newIndex)
        {
            var list = Items.ToList();
            await _controller.ReorderAsync(list, oldIndex, newIndex);
            Items.Clear();
            foreach (var it in list) Items.Add(it);
            RefreshView();
            TouchSaved();
        }

        // ====== Ctor ======
        public MainWindowViewModel(TodoController controller, ITodoStorage storage)
        {
            _controller = controller;
            _storage = storage;

            // ストレージパス表示（Json 実装ならプロパティを足しておくと便利）
            StoragePath = (storage as IStoragePathProvider)?.Path ?? "(local JSON)";

            // --- コマンド定義 ---
            AddCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var title = (NewTitle ?? "").Trim();
                if (string.IsNullOrEmpty(title)) return;

                var item = new TodoItem { Title = title, IsCompleted = false, DueDate = NewDueDate };
                var list = Items.ToList();
                await _controller.AddAsync(list, item);
                Items.Add(item);

                NewTitle = "";
                NewDueDate = null;
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
                item.IsCompleted = !item.IsCompleted;
                var list = Items.ToList();
                await _controller.UpdateAsync(list, item);
                RefreshView();
                TouchSaved();
            });

            EditCommand = ReactiveCommand.CreateFromTask<TodoItem>(async item =>
            {
                // シンプルな編集例：タイトル末尾に " (edited)" を付与
                // 実運用ではダイアログやインライン編集に置き換え
                item.Title = (item.Title ?? "") + " (edited)";
                var list = Items.ToList();
                await _controller.UpdateAsync(list, item);
                RefreshView();
                TouchSaved();
            });
        }

        // 初期化：呼び出し元（Window側）で await LoadAsync() してね
        public async Task LoadAsync()
        {
            var loaded = await _controller.LoadAsync();
            Items.Clear();
            foreach (var it in loaded) Items.Add(it);
            RefreshView();
        }
    }

    // Optional: ストレージのパスをUIに見せたい時の小インタフェース
    public interface IStoragePathProvider { string Path { get; } }
}

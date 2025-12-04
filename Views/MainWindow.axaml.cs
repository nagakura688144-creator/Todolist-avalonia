// Views/MainWindow.axaml.cs
using Avalonia;
using Avalonia.Controls;
using TodoApp.Controllers;
using TodoApp.Services;
using TodoApp.ViewModels;
using TodoApp.Factories;

namespace TodoApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (Design.IsDesignMode)
                return;

            var storage = new JsonTodoStorage();
            var controller = new TodoController(storage);
            var factory = new TodoItemFactory();
            var sortService = new TodoSortService(new DueDateAscendingSortStrategy());
            var validationService = new TodoValidationService();
            var vm = new MainWindowViewModel(controller, storage, factory, sortService, validationService);
            vm.OwnerWindow = this;

            DataContext = vm;

            // 起動後に保存データを読み込み
            this.Opened += async (_, __) => await vm.LoadAsync();
        }
    }
}

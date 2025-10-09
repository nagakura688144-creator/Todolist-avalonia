// Views/MainWindow.axaml.cs
using Avalonia;
using Avalonia.Controls;
using TodoApp.Controllers;
using TodoApp.Services;
using TodoApp.ViewModels;

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
            var vm = new MainWindowViewModel(controller, storage);

            DataContext = vm;

            // 起動後に保存データを読み込み
            this.Opened += async (_, __) => await vm.LoadAsync();
        }
    }
}

using Avalonia.Controls;
using TodolistAvalonia.Services;
using TodolistAvalonia.Controllers;   // 使っている場合
using TodolistAvalonia.ViewModels;

namespace TodolistAvalonia.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var storage = new JsonTodoStorage("TodoApp");   // 保存先フォルダ名
            // Controller を使う設計なら
            var controller = new TodoController(storage);
            var vm = new MainWindowViewModel(controller, storage);

            DataContext = vm;
            this.Opened += async (_, __) => await vm.LoadAsync();
        }
    }
}

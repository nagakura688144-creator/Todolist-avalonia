using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TodoApp.Views;
using TodoApp.Services;
using TodoApp.Controllers;
using TodoApp.ViewModels;

namespace TodoApp
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var storage    = new JsonTodoStorage();
                var controller = new TodoController(storage);
                var vm         = new MainWindowViewModel(controller, storage);

                desktop.MainWindow = new MainWindow { DataContext = vm };
                _ = vm.LoadAsync();
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}

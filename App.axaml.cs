using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TodoApp.Views;
using TodoApp.Services;
using TodoApp.Controllers;
using TodoApp.ViewModels;
using TodoApp.Factories;

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
                // Dependency Injection setup
                var storage = new JsonTodoStorage();
                var controller = new TodoController(storage);
                var factory = new TodoItemFactory();
                var sortService = new TodoSortService(new DueDateAscendingSortStrategy());
                var validationService = new TodoValidationService();
                
                var vm = new MainWindowViewModel(
                    controller, 
                    storage, 
                    factory, 
                    sortService, 
                    validationService);

                var mainWindow = new MainWindow { DataContext = vm };
                vm.OwnerWindow = mainWindow;
                desktop.MainWindow = mainWindow;
                _ = vm.LoadAsync();
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}

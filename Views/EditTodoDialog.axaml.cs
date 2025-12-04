// Views/EditTodoDialog.axaml.cs
using Avalonia.Controls;
using Avalonia.Interactivity;
using TodoApp.ViewModels;

namespace TodoApp.Views
{
    public partial class EditTodoDialog : Window
    {
        public EditTodoDialog()
        {
            InitializeComponent();
        }

        public bool WasSaved { get; private set; }

        private void OnSave(object? sender, RoutedEventArgs e)
        {
            if (DataContext is EditTodoViewModel vm && vm.IsValid)
            {
                vm.ApplyChanges();
                WasSaved = true;
                Close();
            }
        }

        private void OnCancel(object? sender, RoutedEventArgs e)
        {
            WasSaved = false;
            Close();
        }
    }
}

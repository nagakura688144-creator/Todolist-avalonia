// Views/EditTitleDialog.axaml.cs
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace TodoApp.Views
{
    public partial class EditTitleDialog : Window
    {
        public EditTitleDialog()
        {
            InitializeComponent();
            DataContext = this;
        }

        public string? TitleText { get; set; }

        private void OnOk(object? sender, RoutedEventArgs e) => Close(TitleText);
        private void OnCancel(object? sender, RoutedEventArgs e) => Close(null);
    }
}

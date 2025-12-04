// ViewModels/EditTodoViewModel.cs
using ReactiveUI;
using System;
using System.Reactive;
using TodoApp.Models;
using TodoApp.Services;

namespace TodoApp.ViewModels
{
    /// <summary>
    /// ViewModel for editing a TodoItem.
    /// Follows SOLID principles:
    /// - Single Responsibility: Only handles edit dialog logic
    /// - Dependency Inversion: Depends on validation service abstraction
    /// </summary>
    public class EditTodoViewModel : ReactiveObject
    {
        private readonly TodoValidationService _validationService;
        private readonly TodoItem _originalItem;

        private string _title;
        private DateTimeOffset? _dueDate;
        private string? _errorMessage;

        public EditTodoViewModel(TodoItem item, TodoValidationService validationService)
        {
            _originalItem = item;
            _validationService = validationService;

            // Initialize with current values
            _title = item.Title;
            _dueDate = item.DueDate;

            ClearDateCommand = ReactiveCommand.Create(() => { DueDate = null; });
        }

        public string Title
        {
            get => _title;
            set
            {
                this.RaiseAndSetIfChanged(ref _title, value);
                ValidateTitle();
            }
        }

        public DateTimeOffset? DueDate
        {
            get => _dueDate;
            set => this.RaiseAndSetIfChanged(ref _dueDate, value);
        }

        public string? ErrorMessage
        {
            get => _errorMessage;
            private set => this.RaiseAndSetIfChanged(ref _errorMessage, value);
        }

        public bool IsValid => string.IsNullOrEmpty(ErrorMessage) && _validationService.IsValidTitle(Title);

        private void ValidateTitle()
        {
            if (!_validationService.IsValidTitle(Title))
            {
                ErrorMessage = "Title cannot be empty";
            }
            else
            {
                ErrorMessage = null;
            }
            this.RaisePropertyChanged(nameof(IsValid));
        }

        /// <summary>
        /// Applies the changes to the original TodoItem.
        /// </summary>
        public void ApplyChanges()
        {
            if (!IsValid)
            {
                throw new InvalidOperationException("Cannot apply changes when validation fails");
            }

            _originalItem.Title = _validationService.SanitizeTitle(Title);
            _originalItem.DueDate = DueDate;
        }

        public ReactiveCommand<Unit, Unit> ClearDateCommand { get; }
    }
}

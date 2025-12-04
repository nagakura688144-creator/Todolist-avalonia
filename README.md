# TodoApp (Avalonia .NET) - Software Project 2

## Project Overview

This project is an enhanced version of a simple To-Do list application, upgraded to meet the requirements of **Software Project 2**. Originally a basic task manager, it has been significantly refactored to incorporate **SOLID design principles**, a **Creational Design Pattern (Factory)**, and comprehensive **unit testing**. The application is built with **Avalonia UI** and **C# (.NET 8)**, ensuring it is a fast, modern, and cross-platform desktop application that runs on macOS, Windows, and Linux.

Key enhancements include a robust architecture, an advanced **Edit** function, and a powerful **Sort** capability, making it a more feature-rich and maintainable software project.

## Features

- **Add, Edit, and Delete Tasks**: Full CRUD (Create, Read, Update, Delete) functionality for managing tasks.
- **Advanced Edit**: Modify a task's title and due date in a dedicated dialog.
- **Sort by Due Date**: Toggle between sorting tasks by the earliest due date (ascending) and the latest due date (descending). Tasks without a due date are always placed at the bottom.
- **Mark Complete/Incomplete**: Easily track task status with a checkbox.
- **Filtering**: View tasks by **All**, **Active**, or **Completed** status.
- **Due Date Management**: Set optional due dates. An overdue badge appears in **red** for tasks past their due date.
- **Persistent Storage**: Tasks are automatically saved to a local JSON file on exit and loaded on startup.
- **Cross-Platform**: Runs on macOS, Windows, and Linux.
- **Dark/Light Theme**: Switch between visual themes.

## SOLID Principles and Design Patterns

This project was refactored to strictly adhere to SOLID principles and includes a creational design pattern, forming the core of its robust architecture.

### 1. Single Responsibility Principle (SRP)

Each class has a single, well-defined responsibility. Logic that was previously mixed in the `MainWindowViewModel` has been separated into dedicated services:

- **`TodoValidationService`**: Responsible only for validating `TodoItem` data, such as ensuring titles are not empty.
- **`TodoSortService`**: Manages the sorting of `TodoItem` lists by delegating to a specific `ISortStrategy`.
- **`TodoItemFactory`**: Solely responsible for the creation of `TodoItem` objects, ensuring they are instantiated correctly.
- **`TodoController`**: Acts as a mediator between the ViewModel and the storage layer, handling the core business logic of CRUD operations.

### 2. Open/Closed Principle (OCP)

The application is open for extension but closed for modification. This is primarily achieved using the **Strategy Pattern** for sorting:

- **`ISortStrategy`**: An interface that defines a `Sort` method. New sorting algorithms (e.g., by title, by creation date) can be added by creating new classes that implement this interface without altering the existing `TodoSortService` or ViewModel.
- **Implementations**: `DueDateAscendingSortStrategy` and `DueDateDescendingSortStrategy` are concrete implementations that provide different sorting behaviors.

### 3. Liskov Substitution Principle (LSP)

Subtypes are substitutable for their base types. The `ISortStrategy` implementations can be used interchangeably wherever the interface is expected, and the `InMemoryTodoStorage` (used for testing) is a perfect substitute for `ITodoStorage` without altering the `TodoController`'s behavior.

### 4. Interface Segregation Principle (ISP)

Interfaces are small and focused. Clients only depend on the methods they use.

- **`ITodoStorage`**: Defines a minimal contract for loading and saving tasks (`Load`, `Save`).
- **`IStoragePathProvider`**: A separate, small interface for exposing the storage file path, implemented only by classes that need to provide this information.
- **`ITodoItemFactory`**: A focused interface for object creation.

### 5. Dependency Inversion Principle (DIP)

High-level modules do not depend on low-level modules; both depend on abstractions. This is achieved through **Dependency Injection (DI)**:

- The `MainWindowViewModel` depends on abstractions like `ITodoStorage`, `ITodoItemFactory`, and `TodoSortService`, not on concrete implementations like `JsonTodoStorage`.
- All dependencies are provided via the constructor, making the system loosely coupled, more testable, and easier to maintain.

### Creational Design Pattern: Factory Pattern

- **`TodoItemFactory`**: Implements the Factory Pattern to encapsulate the logic of creating `TodoItem` objects. This ensures that all `TodoItem` instances are created consistently, with valid state (e.g., a non-empty title and a new `Guid`). It decouples the ViewModel from the concrete `TodoItem` class's construction logic.

## File Structure

The project is organized into multiple directories, reflecting the MVVM pattern and a clean separation of concerns.

```
Todolist-avalonia/
├── TodoApp/                  # Main Application Project
│   ├── Models/
│   │   └── TodoItem.cs
│   ├── Controllers/
│   │   └── TodoController.cs
│   ├── Services/
│   │   ├── ITodoStorage.cs
│   │   ├── JsonTodoStorage.cs
│   │   ├── ISortStrategy.cs
│   │   ├── DueDateAscendingSortStrategy.cs
│   │   ├── DueDateDescendingSortStrategy.cs
│   │   ├── TodoSortService.cs
│   │   └── TodoValidationService.cs
│   ├── Factories/
│   │   ├── ITodoItemFactory.cs
│   │   └── TodoItemFactory.cs
│   ├── ViewModels/
│   │   ├── ViewModelBase.cs
│   │   ├── MainWindowViewModel.cs
│   │   └── EditTodoViewModel.cs
│   ├── Views/
│   │   ├── MainWindow.axaml
│   │   └── EditTodoDialog.axaml
│   ├── Converters/
│   ├── TodoApp.csproj
│   └── ...
├── TodoApp.Tests/            # Unit Test Project
│   ├── Models/
│   │   └── TodoItemTests.cs
│   ├── Controllers/
│   │   └── TodoControllerTests.cs
│   ├── Services/
│   │   ├── SortStrategyTests.cs
│   │   └── TodoValidationServiceTests.cs
│   ├── Factories/
│   │   └── TodoItemFactoryTests.cs
│   └── TodoApp.Tests.csproj
└── README.md
```

## Unit Testing

The project includes a dedicated test project (`TodoApp.Tests`) with **7 unit tests** to ensure code quality and correctness. The tests cover critical components of the application.

- **`TodoItemFactoryTests` (3 tests)**: Verify that the `TodoItemFactory` correctly creates `TodoItem` objects with valid states, handles edge cases like empty titles, and assigns unique IDs.
- **`TodoControllerTests` (2 tests)**: Test the core business logic for adding and updating tasks, ensuring the controller correctly interacts with the `ITodoStorage` dependency.
- **`SortStrategyTests` (2 tests)**: Confirm that the sorting strategies correctly order tasks by due date and properly handle tasks without a due date, placing them at the end of the list.

To run the tests, navigate to the root directory and execute:

```bash
dotnet test
```

## Installation and Running

### Prerequisites
- **.NET 8 SDK**

### Build & Run (Development)

1. Clone the repository:
   ```bash
   git clone https://github.com/nagakura688144-creator/Todolist-avalonia.git
   cd Todolist-avalonia
   ```

2. Restore dependencies and run the application:
   ```bash
   dotnet restore
   dotnet run --project TodoApp/TodoApp.csproj
   ```

## How Data Is Stored

- **Format**: Data is stored as a JSON array of task objects.
- **Location**: The data file (`todos.json`) is stored in the standard application data directory for the user's operating system (e.g., `~/Library/Application Support/TodoApp` on macOS, `%AppData%\TodoApp` on Windows).

## Debugging Summary

During development, several challenges were addressed:

1.  **.NET SDK Not Found**: The initial sandbox environment did not have the .NET SDK installed. This was resolved by downloading and executing the official `dotnet-install.sh` script to set up the required .NET 8 environment.

2.  **Build Failures due to Project References**: When the `TodoApp.Tests` project was created, the main `TodoApp` project incorrectly tried to compile the test files, leading to numerous compilation errors (e.g., `FactAttribute not found`). This was resolved by explicitly excluding the test files from the main project's `.csproj` file:
    ```xml
    <ItemGroup>
      <Compile Remove="TodoApp.Tests/**/*.cs" />
    </ItemGroup>
    ```

3.  **Dialog Ownership**: The `EditTodoDialog` required a reference to its parent window to display correctly as a modal dialog. This was fixed by passing a reference of the `MainWindow` to the `MainWindowViewModel` and using it when calling `dialog.ShowDialog(OwnerWindow)`.

## Release

A release page for version `v2.0.0` will be created on GitHub, containing installable packages for macOS and Windows.

## Credits and Acknowledgements

- **Avalonia UI**: For the excellent cross-platform UI framework.
- **.NET Team**: For the powerful and versatile .NET 8 SDK.
- **AI Assistants**: For providing guidance on architecture, debugging, and implementation.

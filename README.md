# TodoApp (Avalonia .NET)

## Project Overview
A simple cross-platform To-Do list app built with **Avalonia UI** and **C# (.NET 8)**.  
Create, edit, complete, filter tasks and switch between dark/light themes.  
📝Why useful: (e.g., “I needed a tiny, fast desktop to-do that works on Windows/Mac.”)

## Features
- Add / Edit / Delete tasks
- Mark complete / uncomplete
- Filters: **All / Active / Completed**
- Optional due date (pill shows **red** when overdue)
- Dark / Light theme toggle
- Enter key adds a task
- Persistent storage to local JSON

## File Structure
TodoApp/
├─ App.axaml # App styles & resources
├─ Program.cs
├─ Models/
│ └─ TodoItem.cs
├─ Controllers/
│ └─ TodoController.cs
├─ Services/
│ ├─ Storage.cs # ITodoStorage, JsonTodoStorage
│ └─ IStoragePathProvider.cs
├─ ViewModels/
│ └─ MainWindowViewModel.cs
├─ Views/
│ ├─ MainWindow.axaml
│ ├─ MainWindow.axaml.cs
│ ├─ EditTitleDialog.axaml
│ └─ EditTitleDialog.axaml.cs
├─ Converters/
│ ├─ BoolToTextDecorConverter.cs
│ ├─ BoolToOpacityConverter.cs
│ └─ BoolToBrushConverter.cs
├─ Styles.axaml
└─ README.md

## Installation / Run
### Prerequisites
- .NET 8 SDK
- macOS / Windows / Linux

### Build & Run (dev)
```bash
dotnet restore
dotnet build
dotnet run -c Release
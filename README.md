# TodoApp (Avalonia .NET)

## Project Overview
A simple, fast, cross-platform To-Do app built with **Avalonia UI** and **C# (.NET 8)**.  
Create, edit, complete, filter tasks, set optional due dates, and switch dark/light themes.  
**Why it’s useful:** lightweight desktop to-do that runs on **macOS / Windows / Linux** and stores data locally.

## Features
- Add / Edit / Delete tasks
- Mark complete / uncomplete
- Filters: **All / Active / Completed**
- Optional **due date** (badge turns **red** when overdue)
- **Dark / Light** theme toggle
- **Enter** key adds a task
- **Persistent storage** to local JSON (auto-load on start, auto-save on exit)

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


## Installation (End Users)
### macOS (recommended)
1. Download the **`.pkg` installer** from the Release page:  
   👉 `https://github.com/<your-account>/Todolist-avalonia/releases/tag/v1.0.0`
2. Open the `.pkg` and follow the wizard. The app installs to **/Applications/TodoApp.app**.
3. First launch on macOS may be blocked (Gatekeeper). If so:  
   **System Settings → Privacy & Security → “TodoApp.pkg was blocked” → *Open Anyway***  
   または Finder で **TodoApp を右クリック → 開く**。

### Windows
- (Optional for class) Download the **win-x64 self-contained zip** from Release (if provided), unzip, run `TodoApp.exe`.

### Linux
- Run via `dotnet run` (dev) or publish a self-contained build (see Dev section).

## Installation / Run (Developers)
### Prerequisites
- **.NET 8 SDK**
- macOS / Windows / Linux

### Build & Run (dev)
```bash
dotnet restore
dotnet build
dotnet run -c Release

Publish (self-contained examples)
# macOS Apple Silicon
dotnet publish -c Release -r osx-arm64 --self-contained true

# Windows
dotnet publish -c Release -r win-x64 --self-contained true

macOS Installer (.pkg) – how we built it

For repeatable packaging, we build the app bundle and then a .pkg:

# Build (self-contained)
dotnet publish -c Release -r osx-arm64 --self-contained true

# If your build already creates TodoApp.app, skip the next block.
# Otherwise, assemble a minimal .app bundle (Info.plist + MacOS payload).

# Create installer from the .app bundle (install target: /Applications)
productbuild --component "./TodoApp.app" /Applications "TodoApp-<YYYYMMDD>-mac.pkg"


First launch may require right-click → Open due to the app being unsigned for coursework.

API Usage Details

This app does not call external web APIs. It uses:

Avalonia UI for cross-platform UI

ReactiveUI patterns (where applicable)

System.Text.Json for local persistence

If you add APIs later, document endpoints, keys, and request/response examples here.

How Data Is Stored

Format: JSON array of tasks
Example:

[
  { "id": "guid", "title": "Buy milk", "isDone": false, "due": "2025-10-09" }
]


macOS path: ~/Library/Application Support/TodoApp/tasks.json

Windows path: %AppData%\TodoApp\tasks.json

Linux path: ~/.local/share/TodoApp/tasks.json

Data is auto-loaded on startup and auto-saved on exit.

Known Issues / Limitations

macOS Gatekeeper may block first launch (unsigned app). Use right-click → Open.

No cloud sync (local JSON only).

Single window; no multi-workspace yet.

Debugging Summary

Gatekeeper block: Users could not open .pkg or .app.
Fix: Instructed to allow in Privacy & Security or use xattr -dr com.apple.quarantine.

Missing save file/dir: On first run, storage directory might not exist.
Fix: Create directory on startup; handle file-not-found with empty list.

Overdue badge logic: Date parsing edge cases fixed by normalizing to local date.

Release

GitHub Release (installer attached):
https://github.com/<your-account>/Todolist-avalonia/releases/tag/v1.0.0

Assets include:

TodoApp-<YYYYMMDD>-mac.pkg (macOS installer)

(optional) TodoApp-win-x64.zip (Windows self-contained)

Credits & Acknowledgements

Avalonia UI documentation & samples

.NET / C# docs

Class materials and reviewers
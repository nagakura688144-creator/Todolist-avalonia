# Software Project 2 - Implementation Plan

## 課題要件チェックリスト

### 必須要件
- [ ] 最低2つのユニットテスト
- [ ] GitHubで共有、最低1つの実験的ブランチ
- [ ] SOLID原則の使用
- [ ] ユーザー入力の保存
- [ ] GUI
- [ ] 複数のディレクトリとファイル構造
- [ ] 包括的なREADME.md

### オプション要件
- [ ] Creational Design Pattern（Factory/Builder）

### 追加機能要件
- [ ] Edit機能（Todo項目の名前と日程の変更）
- [ ] ソート機能（日程の昇順/降順、日程なしは最下部）

## SOLID原則の適用計画

### 1. Single Responsibility Principle (SRP)
**現状:** 各クラスは概ね単一責任だが、ViewModelが若干肥大化
**改善:**
- `TodoSortService` - ソートロジックを分離
- `TodoValidationService` - バリデーションロジックを分離

### 2. Open/Closed Principle (OCP)
**現状:** 拡張性は良好
**改善:**
- `ISortStrategy` インターフェースで複数のソート戦略を実装可能に
- `IFilterStrategy` でフィルタリング戦略を拡張可能に

### 3. Liskov Substitution Principle (LSP)
**現状:** インターフェース実装は適切
**改善:**
- ストレージ実装の抽象化を維持

### 4. Interface Segregation Principle (ISP)
**現状:** `ITodoStorage` は適切にシンプル
**改善:**
- `IStoragePathProvider` を分離済み（良好）
- 必要に応じて `IReadOnlyTodoStorage` と `IWritableTodoStorage` に分離

### 5. Dependency Inversion Principle (DIP)
**現状:** 依存性注入を使用中
**改善:**
- すべての依存関係をインターフェース経由に

## Creational Design Pattern: Factory Pattern

### TodoItemFactory
```csharp
public interface ITodoItemFactory
{
    TodoItem Create(string title, DateTimeOffset? dueDate = null);
    TodoItem CreateWithDefaults();
}

public class TodoItemFactory : ITodoItemFactory
{
    public TodoItem Create(string title, DateTimeOffset? dueDate = null)
    {
        return new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = title,
            DueDate = dueDate,
            IsCompleted = false
        };
    }

    public TodoItem CreateWithDefaults()
    {
        return new TodoItem
        {
            Id = Guid.NewGuid(),
            Title = "New Task",
            DueDate = null,
            IsCompleted = false
        };
    }
}
```

## 新機能実装計画

### 1. Edit機能

**必要なコンポーネント:**
- `Views/EditTodoDialog.axaml` - 編集ダイアログUI
- `ViewModels/EditTodoViewModel.cs` - 編集用ViewModel
- `MainWindowViewModel.EditCommand` の実装

**実装手順:**
1. EditTodoDialog作成（Title, DueDateの編集フィールド）
2. EditTodoViewModel作成（バリデーション含む）
3. MainWindowViewModelのEditCommandを実装
4. TodoControllerにUpdateメソッド追加（既存のUpdateAsyncを拡張）

### 2. ソート機能

**必要なコンポーネント:**
- `Services/ISortStrategy.cs` - ソート戦略インターフェース
- `Services/DueDateSortStrategy.cs` - 日程ソート実装
- `Services/TodoSortService.cs` - ソートサービス
- UI: ソートボタン（昇順/降順切り替え）

**ソートロジック:**
```
1. DueDateがnullでない項目を日程順にソート（昇順/降順）
2. DueDateがnullの項目を最後に配置
3. 同じ日程の場合はTitle順
```

**実装手順:**
1. ISortStrategy インターフェース定義
2. DueDateAscendingSortStrategy 実装
3. DueDateDescendingSortStrategy 実装
4. TodoSortService 作成
5. ViewModelにソートコマンド追加
6. UIにソートボタン追加

## ユニットテスト計画

### テストプロジェクト構造
```
TodoApp.Tests/
├── TodoApp.Tests.csproj
├── Models/
│   └── TodoItemTests.cs
├── Controllers/
│   └── TodoControllerTests.cs
├── Services/
│   ├── JsonTodoStorageTests.cs
│   ├── TodoSortServiceTests.cs
│   └── SortStrategyTests.cs
└── Factories/
    └── TodoItemFactoryTests.cs
```

### 最低限のテストケース（2つ以上）
1. **TodoItemFactoryTests.Create_ShouldReturnValidTodoItem**
   - Factoryが正しくTodoItemを生成することを確認

2. **TodoControllerTests.AddAsync_ShouldAddItemAndSave**
   - Todo項目の追加と保存が正しく動作することを確認

3. **DueDateSortStrategyTests.Sort_ShouldPlaceNullDueDatesLast** (追加)
   - ソート時に日程なし項目が最後に来ることを確認

4. **TodoControllerTests.UpdateAsync_ShouldUpdateItemAndSave** (追加)
   - 編集機能が正しく動作することを確認

## ファイル構造（最終形）

```
TodoApp/
├── Models/
│   └── TodoItem.cs
├── Controllers/
│   └── TodoController.cs
├── Services/
│   ├── ITodoStorage.cs
│   ├── JsonTodoStorage.cs
│   ├── ISortStrategy.cs
│   ├── DueDateAscendingSortStrategy.cs
│   ├── DueDateDescendingSortStrategy.cs
│   ├── TodoSortService.cs
│   └── TodoValidationService.cs
├── Factories/
│   ├── ITodoItemFactory.cs
│   └── TodoItemFactory.cs
├── ViewModels/
│   ├── ViewModelBase.cs
│   ├── MainWindowViewModel.cs
│   └── EditTodoViewModel.cs
├── Views/
│   ├── MainWindow.axaml
│   ├── MainWindow.axaml.cs
│   ├── EditTodoDialog.axaml
│   └── EditTodoDialog.axaml.cs
├── Converters/
│   ├── BoolToTextDecorConverter.cs
│   ├── BoolToOpacityConverter.cs
│   ├── BoolToBrushConverter.cs
│   └── BoolToThemeVariantConverter.cs
├── TodoApp.csproj
├── Program.cs
├── App.axaml
├── App.axaml.cs
└── README.md

TodoApp.Tests/
├── TodoApp.Tests.csproj
├── Models/
│   └── TodoItemTests.cs
├── Controllers/
│   └── TodoControllerTests.cs
├── Services/
│   ├── JsonTodoStorageTests.cs
│   └── TodoSortServiceTests.cs
└── Factories/
    └── TodoItemFactoryTests.cs
```

## 実装順序

1. **Phase 3: SOLID原則リファクタリング**
   - Factory Pattern実装
   - Sort Service分離
   - Validation Service分離

2. **Phase 4: 新機能追加**
   - Edit機能実装
   - Sort機能実装

3. **Phase 5: ユニットテスト**
   - テストプロジェクト作成
   - 最低4つのテストケース実装

4. **Phase 6: ドキュメント更新**
   - README.md更新
   - デバッグサマリー追加

5. **Phase 7: リリース準備**
   - ブランチマージ
   - GitHub Release作成

## Git戦略

- `main` - 安定版
- `feature/software-project-2-enhancements` - 実験的ブランチ（現在）
  - 全ての新機能をこのブランチで開発
  - テスト完了後にmainにマージ

## 成功基準

- [ ] すべての要件を満たす
- [ ] ユニットテストがすべてパス
- [ ] アプリケーションが正常に動作
- [ ] README.mdが完全
- [ ] GitHubにリリースページが存在

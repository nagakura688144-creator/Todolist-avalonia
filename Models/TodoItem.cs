using System;
using System.Text.Json.Serialization;
// ★ namespace はあなたのプロジェクトに合わせる（例: TodolistAvalonia.Models）
namespace TodolistAvalonia.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "";
        public bool IsCompleted { get; set; }
        public DateTimeOffset? DueDate { get; set; }   // ← 期日

        [JsonIgnore] public string DueLabel => DueDate.HasValue ? $"Due: {DueDate.Value:MMM dd}" : "No due";
        [JsonIgnore] public bool IsOverdue => DueDate.HasValue && !IsCompleted && DueDate.Value.Date < DateTimeOffset.Now.Date;
    }
}

using System;
using System.Text.Json.Serialization;

namespace TodoApp.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = "";
        public bool IsCompleted { get; set; }
        public DateTimeOffset? DueDate { get; set; }

        [JsonIgnore] public string DueLabel => DueDate.HasValue ? $"Due: {DueDate.Value:MMM dd}" : "No due";
        [JsonIgnore] public bool IsOverdue => DueDate.HasValue && !IsCompleted && DueDate.Value.Date < DateTimeOffset.Now.Date;
    }
}

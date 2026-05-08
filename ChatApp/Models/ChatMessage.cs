namespace ChatApp.Models;

[SQLite.Table("ChatMessage")]
public class ChatMessage
{
    [SQLite.PrimaryKey]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [SQLite.Indexed]
    public string ChatThreadId { get; set; } = string.Empty;

    [SQLite.MaxLength(4000)]
    public string Text { get; set; } = string.Empty;

    public bool IsOutgoing { get; set; }

    [SQLite.Indexed]
    public DateTimeOffset SentAt { get; set; } = DateTimeOffset.UtcNow;

    public string DeliveryTick => IsOutgoing ? "✓✓" : string.Empty;

    [SQLite.Ignore]
    public bool IsSelected { get; set; }
}

namespace ChatApp.Data.Entities;

public class ChatMessageEntity
{
    [SQLite.PrimaryKey]
    public string Id { get; set; } = string.Empty;

    [SQLite.Indexed]
    public string ChatThreadId { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;
    public bool IsOutgoing { get; set; }
    public DateTimeOffset SentAt { get; set; }
}

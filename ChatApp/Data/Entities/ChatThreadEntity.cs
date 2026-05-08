namespace ChatApp.Data.Entities;

public class ChatThreadEntity
{
    [SQLite.PrimaryKey]
    public string Id { get; set; } = string.Empty;

    [SQLite.MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [SQLite.MaxLength(30)]
    public string ParticipantPhoneNumber { get; set; } = string.Empty;

    public string LastMessagePreview { get; set; } = string.Empty;
    public DateTimeOffset UpdatedAt { get; set; }
}

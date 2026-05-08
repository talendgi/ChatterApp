namespace ChatApp.Data.Entities;

public class UserSessionEntity
{
    [SQLite.PrimaryKey]
    public string Id { get; set; } = string.Empty;

    [SQLite.MaxLength(30)]
    public string PhoneNumber { get; set; } = string.Empty;

    [SQLite.MaxLength(100)]
    public string DisplayName { get; set; } = string.Empty;

    public bool IsLoggedIn { get; set; }
    public DateTimeOffset LastLoginAt { get; set; }
}

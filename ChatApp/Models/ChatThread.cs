namespace ChatApp.Models;

public class ChatThread
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Title { get; set; } = string.Empty;
    public string ParticipantPhoneNumber { get; set; } = string.Empty;
    public string LastMessagePreview { get; set; } = string.Empty;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    public string AvatarInitial => string.IsNullOrWhiteSpace(Title) ? "C" : Title.Trim()[0].ToString().ToUpperInvariant();
}

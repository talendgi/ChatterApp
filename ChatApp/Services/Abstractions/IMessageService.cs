namespace ChatApp.Services.Abstractions;

public interface IMessageService
{
    Task<IReadOnlyList<Models.ChatMessage>> GetMessagesAsync(string chatThreadId, CancellationToken cancellationToken = default);
    Task SendMessageAsync(string chatThreadId, string text, CancellationToken cancellationToken = default);
    Task EditMessageAsync(string messageId, string updatedText, CancellationToken cancellationToken = default);
    Task DeleteMessageAsync(string messageId, CancellationToken cancellationToken = default);
    Task DeleteChatHistoryAsync(string chatThreadId, CancellationToken cancellationToken = default);
}

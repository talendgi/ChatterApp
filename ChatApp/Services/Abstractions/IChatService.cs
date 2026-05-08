namespace ChatApp.Services.Abstractions;

public interface IChatService
{
    Task<IReadOnlyList<Models.ChatThread>> GetChatThreadsAsync(CancellationToken cancellationToken = default);
    Task<Models.ChatThread> CreateOrGetDirectChatAsync(string phoneNumber, CancellationToken cancellationToken = default);
}

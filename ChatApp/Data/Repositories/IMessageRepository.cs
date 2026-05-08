namespace ChatApp.Data.Repositories;

public interface IMessageRepository
{
    Task<IReadOnlyList<Data.Entities.ChatMessageEntity>> GetByThreadIdAsync(string chatThreadId, CancellationToken cancellationToken = default);
    Task<Data.Entities.ChatMessageEntity?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task InsertAsync(Data.Entities.ChatMessageEntity message, CancellationToken cancellationToken = default);
    Task UpdateAsync(Data.Entities.ChatMessageEntity message, CancellationToken cancellationToken = default);
    Task DeleteAsync(string id, CancellationToken cancellationToken = default);
    Task DeleteByThreadIdAsync(string chatThreadId, CancellationToken cancellationToken = default);
}

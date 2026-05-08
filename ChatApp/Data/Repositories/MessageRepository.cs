using ChatApp.Data.Database;

namespace ChatApp.Data.Repositories;

public class MessageRepository(AppDatabase database) : IMessageRepository
{
    private readonly AppDatabase _database = database;

    public async Task<IReadOnlyList<Data.Entities.ChatMessageEntity>> GetByThreadIdAsync(string chatThreadId, CancellationToken cancellationToken = default)
    {
        await _database.InitializeAsync();
        var items = await _database.Connection.Table<Data.Entities.ChatMessageEntity>()
            .Where(x => x.ChatThreadId == chatThreadId)
            .OrderBy(x => x.SentAt)
            .ToListAsync();

        return items;
    }

    public async Task<Data.Entities.ChatMessageEntity?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        await _database.InitializeAsync();
        return await _database.Connection.Table<Data.Entities.ChatMessageEntity>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task InsertAsync(Data.Entities.ChatMessageEntity message, CancellationToken cancellationToken = default)
    {
        await _database.InitializeAsync();
        await _database.Connection.InsertAsync(message);
    }

    public async Task UpdateAsync(Data.Entities.ChatMessageEntity message, CancellationToken cancellationToken = default)
    {
        await _database.InitializeAsync();
        await _database.Connection.UpdateAsync(message);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        await _database.InitializeAsync();
        await _database.Connection.Table<Data.Entities.ChatMessageEntity>()
            .DeleteAsync(x => x.Id == id);
    }

    public async Task DeleteByThreadIdAsync(string chatThreadId, CancellationToken cancellationToken = default)
    {
        await _database.InitializeAsync();
        await _database.Connection.Table<Data.Entities.ChatMessageEntity>()
            .DeleteAsync(x => x.ChatThreadId == chatThreadId);
    }
}

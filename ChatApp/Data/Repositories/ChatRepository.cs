using ChatApp.Data.Database;

namespace ChatApp.Data.Repositories;

public class ChatRepository(AppDatabase database) : IChatRepository
{
    private readonly AppDatabase _database = database;

    public async Task<IReadOnlyList<Data.Entities.ChatThreadEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await _database.InitializeAsync();
        var items = await _database.Connection.Table<Data.Entities.ChatThreadEntity>()
            .OrderByDescending(x => x.UpdatedAt)
            .ToListAsync();

        return items;
    }

    public async Task<Data.Entities.ChatThreadEntity?> GetByPhoneAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        await _database.InitializeAsync();
        return await _database.Connection.Table<Data.Entities.ChatThreadEntity>()
            .FirstOrDefaultAsync(x => x.ParticipantPhoneNumber == phoneNumber);
    }

    public async Task InsertAsync(Data.Entities.ChatThreadEntity chatThread, CancellationToken cancellationToken = default)
    {
        await _database.InitializeAsync();
        await _database.Connection.InsertAsync(chatThread);
    }

    public async Task UpdateAsync(Data.Entities.ChatThreadEntity chatThread, CancellationToken cancellationToken = default)
    {
        await _database.InitializeAsync();
        await _database.Connection.UpdateAsync(chatThread);
    }
}

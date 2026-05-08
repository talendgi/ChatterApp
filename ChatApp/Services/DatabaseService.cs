using SQLite;

namespace ChatApp.Services;

public class DatabaseService
{
    private const string DatabaseFileName = "chatapp.db3";

    private readonly SemaphoreSlim _initLock = new(1, 1);
    private SQLiteAsyncConnection? _connection;

    public async Task InitializeAsync()
    {
        if (_connection is not null)
        {
            return;
        }

        await _initLock.WaitAsync();
        try
        {
            if (_connection is not null)
            {
                return;
            }

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, DatabaseFileName);
            _connection = new SQLiteAsyncConnection(
                dbPath,
                SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);

            await _connection.CreateTableAsync<Models.ChatMessage>();
            await _connection.ExecuteAsync("CREATE INDEX IF NOT EXISTS IX_ChatMessage_ChatThreadId_SentAt ON ChatMessage(ChatThreadId, SentAt)");
        }
        finally
        {
            _initLock.Release();
        }
    }

    public async Task<int> InsertMessageAsync(Models.ChatMessage message, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(message);
        await EnsureInitializedAsync(cancellationToken);

        message.Id = string.IsNullOrWhiteSpace(message.Id) ? Guid.NewGuid().ToString() : message.Id;
        if (message.SentAt == default)
        {
            message.SentAt = DateTimeOffset.UtcNow;
        }

        return await _connection!.InsertAsync(message);
    }

    public async Task<IReadOnlyList<Models.ChatMessage>> GetMessagesByChatAsync(string chatThreadId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(chatThreadId))
        {
            return [];
        }

        await EnsureInitializedAsync(cancellationToken);

        var messages = await _connection!.Table<Models.ChatMessage>()
            .Where(m => m.ChatThreadId == chatThreadId)
            .OrderBy(m => m.SentAt)
            .ToListAsync();

        return messages;
    }

    private async Task EnsureInitializedAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await InitializeAsync();
        cancellationToken.ThrowIfCancellationRequested();
    }
}

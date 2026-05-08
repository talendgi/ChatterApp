using ChatApp.Core.Constants;

namespace ChatApp.Data.Database;

public class AppDatabase
{
    private SQLite.SQLiteAsyncConnection? _connection;

    public async Task InitializeAsync()
    {
        if (_connection is not null)
        {
            return;
        }

        var databasePath = Path.Combine(FileSystem.AppDataDirectory, DatabaseConstants.DatabaseFileName);
        _connection = new SQLite.SQLiteAsyncConnection(databasePath, SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create | SQLite.SQLiteOpenFlags.SharedCache);

        await _connection.CreateTableAsync<Data.Entities.ChatThreadEntity>();
        await _connection.CreateTableAsync<Data.Entities.ChatMessageEntity>();
        await _connection.CreateTableAsync<Data.Entities.UserSessionEntity>();
        await _connection.CreateTableAsync<Data.Entities.AppSettingsEntity>();

        await SeedAsync();
    }

    public SQLite.SQLiteAsyncConnection Connection => _connection ?? throw new InvalidOperationException("Database is not initialized.");

    private async Task SeedAsync()
    {
        if (_connection is null)
        {
            return;
        }

        var threadCount = await _connection.Table<Data.Entities.ChatThreadEntity>().CountAsync();
        if (threadCount > 0)
        {
            return;
        }

        var now = DateTimeOffset.UtcNow;
        var friendsId = Guid.NewGuid().ToString();
        var familyId = Guid.NewGuid().ToString();

        var threads = new List<Data.Entities.ChatThreadEntity>
        {
            new()
            {
                Id = friendsId,
                Title = "Friends Group",
                ParticipantPhoneNumber = "+1000000001",
                LastMessagePreview = "Movie tonight at 8?",
                UpdatedAt = now.AddMinutes(-12)
            },
            new()
            {
                Id = familyId,
                Title = "Family",
                ParticipantPhoneNumber = "+1000000002",
                LastMessagePreview = "Dinner is ready!",
                UpdatedAt = now.AddMinutes(-28)
            }
        };

        var messages = new List<Data.Entities.ChatMessageEntity>
        {
            new() { Id = Guid.NewGuid().ToString(), ChatThreadId = friendsId, Text = "Hey everyone!", IsOutgoing = false, SentAt = now.AddMinutes(-35) },
            new() { Id = Guid.NewGuid().ToString(), ChatThreadId = friendsId, Text = "Movie tonight at 8?", IsOutgoing = false, SentAt = now.AddMinutes(-12) },
            new() { Id = Guid.NewGuid().ToString(), ChatThreadId = familyId, Text = "Are you coming home soon?", IsOutgoing = false, SentAt = now.AddMinutes(-40) },
            new() { Id = Guid.NewGuid().ToString(), ChatThreadId = familyId, Text = "Dinner is ready!", IsOutgoing = false, SentAt = now.AddMinutes(-28) }
        };

        await _connection.InsertAllAsync(threads);
        await _connection.InsertAllAsync(messages);
    }
}

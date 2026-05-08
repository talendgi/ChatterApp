using ChatApp.Data.Database;
using ChatApp.Services.Abstractions;

namespace ChatApp.Services.Implementations;

public class AppSettingsService(AppDatabase database) : IAppSettingsService
{
    private readonly AppDatabase _database = database;

    public async Task<Models.AppSettings> GetAsync(CancellationToken cancellationToken = default)
    {
        await _database.InitializeAsync();
        var row = await EnsureRowAsync();
        return new Models.AppSettings
        {
            ProfilePhotoPath = row.ProfilePhotoPath,
            ChatBackgroundPath = row.ChatBackgroundPath
        };
    }

    public async Task SaveProfilePhotoAsync(string? path, CancellationToken cancellationToken = default)
    {
        await _database.InitializeAsync();
        var row = await EnsureRowAsync();
        row.ProfilePhotoPath = path;
        await _database.Connection.UpdateAsync(row);
    }

    public async Task SaveChatBackgroundAsync(string? path, CancellationToken cancellationToken = default)
    {
        await _database.InitializeAsync();
        var row = await EnsureRowAsync();
        row.ChatBackgroundPath = path;
        await _database.Connection.UpdateAsync(row);
    }

    private async Task<Data.Entities.AppSettingsEntity> EnsureRowAsync()
    {
        var row = await _database.Connection.Table<Data.Entities.AppSettingsEntity>().FirstOrDefaultAsync(x => x.Id == "app-settings");
        if (row is not null)
        {
            return row;
        }

        row = new Data.Entities.AppSettingsEntity();
        await _database.Connection.InsertAsync(row);
        return row;
    }
}

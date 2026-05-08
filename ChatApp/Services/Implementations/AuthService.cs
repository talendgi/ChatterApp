using ChatApp.Data.Database;
using ChatApp.Services.Abstractions;

namespace ChatApp.Services.Implementations;

public class AuthService(AppDatabase database) : IAuthService
{
    private readonly AppDatabase _database = database;
    private string _pendingPhoneNumber = string.Empty;
    private const string DemoOtp = "123456";

    public async Task<bool> IsLoggedInAsync(CancellationToken cancellationToken = default)
    {
        await _database.InitializeAsync();
        var session = await _database.Connection.Table<Data.Entities.UserSessionEntity>()
            .FirstOrDefaultAsync(x => x.IsLoggedIn);

        return session is not null;
    }

    public Task StartLoginAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        _pendingPhoneNumber = phoneNumber.Trim();
        return Task.CompletedTask;
    }

    public async Task<bool> VerifyOtpAsync(string otpCode, string displayName, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_pendingPhoneNumber) || otpCode.Trim() != DemoOtp)
        {
            return false;
        }

        await _database.InitializeAsync();
        var existing = await _database.Connection.Table<Data.Entities.UserSessionEntity>()
            .FirstOrDefaultAsync();

        var entity = existing ?? new Data.Entities.UserSessionEntity { Id = Guid.NewGuid().ToString() };
        entity.PhoneNumber = _pendingPhoneNumber;
        entity.DisplayName = string.IsNullOrWhiteSpace(displayName) ? "User" : displayName.Trim();
        entity.IsLoggedIn = true;
        entity.LastLoginAt = DateTimeOffset.UtcNow;

        if (existing is null)
        {
            await _database.Connection.InsertAsync(entity);
        }
        else
        {
            await _database.Connection.UpdateAsync(entity);
        }

        _pendingPhoneNumber = string.Empty;
        return true;
    }

    public async Task<string?> GetLoggedInPhoneNumberAsync(CancellationToken cancellationToken = default)
    {
        await _database.InitializeAsync();
        var session = await _database.Connection.Table<Data.Entities.UserSessionEntity>()
            .FirstOrDefaultAsync(x => x.IsLoggedIn);

        return session?.PhoneNumber;
    }

    public async Task LogoutAsync(CancellationToken cancellationToken = default)
    {
        await _database.InitializeAsync();
        var sessions = await _database.Connection.Table<Data.Entities.UserSessionEntity>().ToListAsync();
        foreach (var item in sessions)
        {
            item.IsLoggedIn = false;
            await _database.Connection.UpdateAsync(item);
        }
    }
}

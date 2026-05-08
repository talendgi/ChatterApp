namespace ChatApp.Services.Abstractions;

public interface IAppSettingsService
{
    Task<Models.AppSettings> GetAsync(CancellationToken cancellationToken = default);
    Task SaveProfilePhotoAsync(string? path, CancellationToken cancellationToken = default);
    Task SaveChatBackgroundAsync(string? path, CancellationToken cancellationToken = default);
}

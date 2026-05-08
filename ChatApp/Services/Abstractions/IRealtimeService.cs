namespace ChatApp.Services.Abstractions;

public interface IRealtimeService
{
    Task ConnectAsync(CancellationToken cancellationToken = default);
    Task DisconnectAsync(CancellationToken cancellationToken = default);
}

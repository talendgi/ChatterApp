namespace ChatApp.Services.Implementations;

public class RealtimeServiceStub : Services.Abstractions.IRealtimeService
{
    public Task ConnectAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task DisconnectAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
}

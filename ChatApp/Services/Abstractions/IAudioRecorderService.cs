namespace ChatApp.Services.Abstractions;

public interface IAudioRecorderService
{
    bool IsRecording { get; }
    Task<bool> StartAsync(CancellationToken cancellationToken = default);
    Task<string?> StopAsync(CancellationToken cancellationToken = default);
}

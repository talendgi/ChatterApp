using Plugin.Maui.Audio;

namespace ChatApp.Services.Implementations;

public class AudioRecorderService : Services.Abstractions.IAudioRecorderService
{
    private IAudioRecorder? _recorder;
    private string? _currentFilePath;

    public bool IsRecording => _recorder?.IsRecording ?? false;

    public async Task<bool> StartAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (IsRecording)
            {
                return true;
            }

            _recorder ??= AudioManager.Current.CreateRecorder();

            var status = await Permissions.RequestAsync<Permissions.Microphone>();
            if (status != PermissionStatus.Granted)
            {
                return false;
            }

            // m4a is broadly supported on Android for device recording.
            _currentFilePath = Path.Combine(FileSystem.AppDataDirectory, $"audio_{DateTimeOffset.UtcNow:yyyyMMdd_HHmmss}.m4a");
            await _recorder.StartAsync(_currentFilePath);
            return true;
        }
        catch
        {
            _currentFilePath = null;
            return false;
        }
    }

    public async Task<string?> StopAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (!IsRecording || _recorder is null)
            {
                return null;
            }

            await _recorder.StopAsync();

            var path = _currentFilePath;
            _currentFilePath = null;
            return path;
        }
        catch
        {
            _currentFilePath = null;
            return null;
        }
    }
}

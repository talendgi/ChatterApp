using System.Collections.ObjectModel;

namespace ChatApp.ViewModels;

public partial class ChatDetailViewModel(
    Services.Abstractions.IMessageService messageService,
    Services.Abstractions.IAppSettingsService appSettingsService,
    Services.Abstractions.IAudioRecorderService audioRecorderService) : ViewModels.Base.BaseViewModel, IQueryAttributable
{
    private readonly Services.Abstractions.IMessageService _messageService = messageService;
    private readonly Services.Abstractions.IAppSettingsService _appSettingsService = appSettingsService;
    private readonly Services.Abstractions.IAudioRecorderService _audioRecorderService = audioRecorderService;

    public ObservableCollection<Models.ChatMessage> Messages { get; } = [];

    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private string chatThreadId = string.Empty;

    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private string messageText = string.Empty;

    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private string chatBackgroundPath = string.Empty;

    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private bool isSelectionMode;

    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private bool isRecordingAudio;

    public string RecordButtonText => IsRecordingAudio ? "Stop" : "Mic";

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("chatThreadId", out var value) && value is string id)
        {
            ChatThreadId = id;
        }
    }

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private async Task LoadAsync()
    {
        if (string.IsNullOrWhiteSpace(ChatThreadId))
        {
            return;
        }

        var settings = await _appSettingsService.GetAsync();
        ChatBackgroundPath = settings.ChatBackgroundPath ?? string.Empty;

        Messages.Clear();
        var items = await _messageService.GetMessagesAsync(ChatThreadId);
        foreach (var item in items)
        {
            Messages.Add(item);
        }
    }

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private async Task SendAsync()
    {
        if (string.IsNullOrWhiteSpace(ChatThreadId))
        {
            return;
        }

        await _messageService.SendMessageAsync(ChatThreadId, MessageText);
        MessageText = string.Empty;
        await LoadAsync();
    }

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private async Task AddAttachmentAsync()
    {
        var result = await Shell.Current.DisplayActionSheetAsync("Choose upload type", "Cancel", null, "Image", "Video", "Audio", "Document", "Any file");
        if (result is "Cancel" or null)
        {
            return;
        }

        var file = await FilePicker.Default.PickAsync();
        if (file is null)
        {
            return;
        }

        await _messageService.SendMessageAsync(ChatThreadId, $"[{result}] {file.FileName}");
        await LoadAsync();
    }

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private async Task RecordAudioAsync()
    {
        if (string.IsNullOrWhiteSpace(ChatThreadId))
        {
            return;
        }

        if (!IsRecordingAudio)
        {
            var started = await _audioRecorderService.StartAsync();
            if (!started)
            {
                await Shell.Current.DisplayAlertAsync("Audio unavailable", "Could not start recording. Please allow microphone permission and try again.", "OK");
                return;
            }

            IsRecordingAudio = true;
            OnPropertyChanged(nameof(RecordButtonText));
            return;
        }

        var filePath = await _audioRecorderService.StopAsync();
        IsRecordingAudio = false;
        OnPropertyChanged(nameof(RecordButtonText));

        if (!string.IsNullOrWhiteSpace(filePath))
        {
            await _messageService.SendMessageAsync(ChatThreadId, $"[Audio] {Path.GetFileName(filePath)}");
            await LoadAsync();
            return;
        }

        await Shell.Current.DisplayAlertAsync("Audio unavailable", "Recording stopped but no audio file was produced.", "OK");
    }

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private async Task MessageOptionsAsync(Models.ChatMessage? message)
    {
        if (message is null)
        {
            return;
        }

        if (IsSelectionMode)
        {
            message.IsSelected = !message.IsSelected;
            return;
        }

        var action = await Shell.Current.DisplayActionSheetAsync("Message options", "Cancel", null, "Edit", "Delete");
        if (action == "Delete")
        {
            await _messageService.DeleteMessageAsync(message.Id);
            await LoadAsync();
        }
        else if (action == "Edit")
        {
            var edited = await Shell.Current.DisplayPromptAsync("Edit message", "Update message text:", initialValue: message.Text);
            if (!string.IsNullOrWhiteSpace(edited))
            {
                await _messageService.EditMessageAsync(message.Id, edited);
                await LoadAsync();
            }
        }
    }

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private void ToggleSelectionMode()
    {
        IsSelectionMode = !IsSelectionMode;
        if (!IsSelectionMode)
        {
            foreach (var msg in Messages)
            {
                msg.IsSelected = false;
            }
        }
    }

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private async Task DeleteSelectedAsync()
    {
        var selected = Messages.Where(x => x.IsSelected).ToList();
        if (selected.Count == 0)
        {
            await Shell.Current.DisplayAlertAsync("No selection", "Select at least one message.", "OK");
            return;
        }

        var yes = await Shell.Current.DisplayAlertAsync("Delete selected", $"Delete {selected.Count} selected messages?", "Delete", "Cancel");
        if (!yes)
        {
            return;
        }

        foreach (var msg in selected)
        {
            await _messageService.DeleteMessageAsync(msg.Id);
        }

        IsSelectionMode = false;
        await LoadAsync();
    }

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private async Task DeleteHistoryAsync()
    {
        var yes = await Shell.Current.DisplayAlertAsync("Delete chat history", "Delete all messages in this chat?", "Delete all", "Cancel");
        if (!yes)
        {
            return;
        }

        await _messageService.DeleteChatHistoryAsync(ChatThreadId);
        await LoadAsync();
    }
}

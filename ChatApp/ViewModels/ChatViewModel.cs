using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Message = ChatApp.Models.ChatMessage;

namespace ChatApp.ViewModels;

public partial class ChatViewModel : ObservableObject
{
    private readonly Services.Abstractions.IRealtimeService _realtimeService;
    private readonly SemaphoreSlim _sendLock = new(1, 1);

    public ObservableCollection<Message> Messages { get; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SendMessageCommand))]
    private string messageText = string.Empty;

    [ObservableProperty]
    private string currentChatThreadId = "local-thread";

    [ObservableProperty]
    private bool isBusy;

    public ChatViewModel(Services.Abstractions.IRealtimeService realtimeService)
    {
        _realtimeService = realtimeService;
    }

    private bool CanSendMessage() =>
        !IsBusy &&
        !string.IsNullOrWhiteSpace(CurrentChatThreadId) &&
        !string.IsNullOrWhiteSpace(MessageText);

    [RelayCommand(CanExecute = nameof(CanSendMessage))]
    private async Task SendMessageAsync(CancellationToken cancellationToken)
    {
        var text = MessageText?.Trim();
        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        await _sendLock.WaitAsync(cancellationToken);
        try
        {
            IsBusy = true;

            var sent = CreateMessage(text, isOutgoing: true);
            Messages.Add(sent);

            // Clear input immediately for responsive UI.
            MessageText = string.Empty;

            await PersistOrDispatchOutgoingAsync(sent, cancellationToken);
            await SimulateIncomingReplyAsync(sent, cancellationToken);
        }
        finally
        {
            IsBusy = false;
            _sendLock.Release();
        }
    }

    public async Task InitializeRealtimeAsync(CancellationToken cancellationToken = default)
    {
        await _realtimeService.ConnectAsync(cancellationToken);
    }

    public async Task StopRealtimeAsync(CancellationToken cancellationToken = default)
    {
        await _realtimeService.DisconnectAsync(cancellationToken);
    }

    public void OnIncomingMessageReceived(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        Messages.Add(CreateMessage(text.Trim(), isOutgoing: false));
    }

    private Message CreateMessage(string text, bool isOutgoing) => new()
    {
        Id = Guid.NewGuid().ToString(),
        ChatThreadId = CurrentChatThreadId,
        Text = text,
        IsOutgoing = isOutgoing,
        SentAt = DateTimeOffset.UtcNow
    };

    private async Task PersistOrDispatchOutgoingAsync(Message message, CancellationToken cancellationToken)
    {
        // Extension point for future SignalR + local persistence integration.
        await Task.CompletedTask;
    }

    private async Task SimulateIncomingReplyAsync(Message source, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(900), cancellationToken);

        var replyText = $"Echo: {source.Text}";
        Messages.Add(CreateMessage(replyText, isOutgoing: false));
    }
}

using ChatApp.Data.Repositories;

namespace ChatApp.Services.Implementations;

public class MessageService(IMessageRepository messageRepository, IChatRepository chatRepository) : Services.Abstractions.IMessageService
{
    private readonly IMessageRepository _messageRepository = messageRepository;
    private readonly IChatRepository _chatRepository = chatRepository;

    public async Task<IReadOnlyList<Models.ChatMessage>> GetMessagesAsync(string chatThreadId, CancellationToken cancellationToken = default)
    {
        var entities = await _messageRepository.GetByThreadIdAsync(chatThreadId, cancellationToken);

        return entities.Select(e => new Models.ChatMessage
        {
            Id = e.Id,
            ChatThreadId = e.ChatThreadId,
            Text = e.Text,
            IsOutgoing = e.IsOutgoing,
            SentAt = e.SentAt
        }).ToList();
    }

    public async Task SendMessageAsync(string chatThreadId, string text, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        var message = new Data.Entities.ChatMessageEntity
        {
            Id = Guid.NewGuid().ToString(),
            ChatThreadId = chatThreadId,
            Text = text.Trim(),
            IsOutgoing = true,
            SentAt = DateTimeOffset.UtcNow
        };

        await _messageRepository.InsertAsync(message, cancellationToken);

        var thread = await _chatRepository.GetAllAsync(cancellationToken);
        var current = thread.FirstOrDefault(x => x.Id == chatThreadId);
        if (current is not null)
        {
            current.LastMessagePreview = message.Text;
            current.UpdatedAt = message.SentAt;
            await _chatRepository.UpdateAsync(current, cancellationToken);
        }
    }

    public async Task EditMessageAsync(string messageId, string updatedText, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(messageId) || string.IsNullOrWhiteSpace(updatedText))
        {
            return;
        }

        var message = await _messageRepository.GetByIdAsync(messageId, cancellationToken);
        if (message is null)
        {
            return;
        }

        message.Text = updatedText.Trim();
        await _messageRepository.UpdateAsync(message, cancellationToken);
    }

    public async Task DeleteMessageAsync(string messageId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(messageId))
        {
            return;
        }

        await _messageRepository.DeleteAsync(messageId, cancellationToken);
    }

    public async Task DeleteChatHistoryAsync(string chatThreadId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(chatThreadId))
        {
            return;
        }

        await _messageRepository.DeleteByThreadIdAsync(chatThreadId, cancellationToken);

        var threads = await _chatRepository.GetAllAsync(cancellationToken);
        var thread = threads.FirstOrDefault(x => x.Id == chatThreadId);
        if (thread is not null)
        {
            thread.LastMessagePreview = "No messages yet";
            thread.UpdatedAt = DateTimeOffset.UtcNow;
            await _chatRepository.UpdateAsync(thread, cancellationToken);
        }
    }
}

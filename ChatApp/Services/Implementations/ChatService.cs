using ChatApp.Data.Repositories;

namespace ChatApp.Services.Implementations;

public class ChatService(IChatRepository chatRepository) : Services.Abstractions.IChatService
{
    private readonly IChatRepository _chatRepository = chatRepository;

    public async Task<IReadOnlyList<Models.ChatThread>> GetChatThreadsAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _chatRepository.GetAllAsync(cancellationToken);

        return entities.Select(e => new Models.ChatThread
        {
            Id = e.Id,
            Title = e.Title,
            ParticipantPhoneNumber = e.ParticipantPhoneNumber,
            LastMessagePreview = e.LastMessagePreview,
            UpdatedAt = e.UpdatedAt
        }).ToList();
    }

    public async Task<Models.ChatThread> CreateOrGetDirectChatAsync(string phoneNumber, CancellationToken cancellationToken = default)
    {
        var sanitized = phoneNumber.Trim();
        var existing = await _chatRepository.GetByPhoneAsync(sanitized, cancellationToken);
        if (existing is null)
        {
            existing = new Data.Entities.ChatThreadEntity
            {
                Id = Guid.NewGuid().ToString(),
                Title = sanitized,
                ParticipantPhoneNumber = sanitized,
                LastMessagePreview = "Start chatting",
                UpdatedAt = DateTimeOffset.UtcNow
            };

            await _chatRepository.InsertAsync(existing, cancellationToken);
        }

        return new Models.ChatThread
        {
            Id = existing.Id,
            Title = existing.Title,
            ParticipantPhoneNumber = existing.ParticipantPhoneNumber,
            LastMessagePreview = existing.LastMessagePreview,
            UpdatedAt = existing.UpdatedAt
        };
    }
}

namespace ChatApp.Data.Repositories;

public interface IChatRepository
{
    Task<IReadOnlyList<Data.Entities.ChatThreadEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Data.Entities.ChatThreadEntity?> GetByPhoneAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task InsertAsync(Data.Entities.ChatThreadEntity chatThread, CancellationToken cancellationToken = default);
    Task UpdateAsync(Data.Entities.ChatThreadEntity chatThread, CancellationToken cancellationToken = default);
}

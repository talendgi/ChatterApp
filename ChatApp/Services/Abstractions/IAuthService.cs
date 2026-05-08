namespace ChatApp.Services.Abstractions;

public interface IAuthService
{
    Task<bool> IsLoggedInAsync(CancellationToken cancellationToken = default);
    Task StartLoginAsync(string phoneNumber, CancellationToken cancellationToken = default);
    Task<bool> VerifyOtpAsync(string otpCode, string displayName, CancellationToken cancellationToken = default);
    Task<string?> GetLoggedInPhoneNumberAsync(CancellationToken cancellationToken = default);
    Task LogoutAsync(CancellationToken cancellationToken = default);
}

using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.ViewModels;

public partial class OtpViewModel(Services.Abstractions.IAuthService authService, IServiceProvider serviceProvider) : ViewModels.Base.BaseViewModel
{
    private readonly Services.Abstractions.IAuthService _authService = authService;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private string otpCode = string.Empty;

    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private string displayName = string.Empty;

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private async Task VerifyAsync()
    {
        var ok = await _authService.VerifyOtpAsync(OtpCode, DisplayName);
        if (!ok)
        {
            await Application.Current!.Windows[0].Page!.DisplayAlertAsync("Verification failed", "Use demo OTP: 123456", "OK");
            return;
        }

        var shell = _serviceProvider.GetRequiredService<AppShell>();
        Application.Current!.Windows[0].Page = shell;
    }
}

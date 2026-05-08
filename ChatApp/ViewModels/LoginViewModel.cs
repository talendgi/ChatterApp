using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.ViewModels;

public partial class LoginViewModel(Services.Abstractions.IAuthService authService, IServiceProvider serviceProvider) : ViewModels.Base.BaseViewModel
{
    private readonly Services.Abstractions.IAuthService _authService = authService;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private string phoneNumber = string.Empty;

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private async Task ContinueAsync()
    {
        if (string.IsNullOrWhiteSpace(PhoneNumber))
        {
            await Application.Current!.Windows[0].Page!.DisplayAlertAsync("Invalid", "Enter a mobile number.", "OK");
            return;
        }

        await _authService.StartLoginAsync(PhoneNumber);
        var otpPage = _serviceProvider.GetRequiredService<Views.Pages.OtpPage>();
        await Application.Current!.Windows[0].Page!.Navigation.PushAsync(otpPage);
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace ChatApp.ViewModels;

public partial class SettingsViewModel(
    Services.Abstractions.IAppSettingsService appSettingsService,
    Services.Abstractions.IAuthService authService,
    IServiceProvider serviceProvider) : ViewModels.Base.BaseViewModel
{
    private readonly Services.Abstractions.IAppSettingsService _appSettingsService = appSettingsService;
    private readonly Services.Abstractions.IAuthService _authService = authService;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private string profilePhotoPath = string.Empty;

    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private string chatBackgroundPath = string.Empty;

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private async Task LoadAsync()
    {
        var data = await _appSettingsService.GetAsync();
        ProfilePhotoPath = data.ProfilePhotoPath ?? string.Empty;
        ChatBackgroundPath = data.ChatBackgroundPath ?? string.Empty;
    }

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private async Task PickProfilePhotoAsync()
    {
        var picked = await FilePicker.Default.PickAsync();
        if (picked is null)
        {
            return;
        }

        ProfilePhotoPath = picked.FullPath;
        await _appSettingsService.SaveProfilePhotoAsync(ProfilePhotoPath);
    }

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private async Task PickBackgroundAsync()
    {
        var picked = await FilePicker.Default.PickAsync();
        if (picked is null)
        {
            return;
        }

        ChatBackgroundPath = picked.FullPath;
        await _appSettingsService.SaveChatBackgroundAsync(ChatBackgroundPath);
    }

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private async Task SetSampleBackgroundAsync(string sampleName)
    {
        ChatBackgroundPath = sampleName;
        await _appSettingsService.SaveChatBackgroundAsync(ChatBackgroundPath);
    }

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private async Task SetSampleAvatarAsync(string sampleName)
    {
        ProfilePhotoPath = sampleName;
        await _appSettingsService.SaveProfilePhotoAsync(ProfilePhotoPath);
    }

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private async Task LogoutAsync()
    {
        await _authService.LogoutAsync();
        var loginPage = _serviceProvider.GetRequiredService<Views.Pages.LoginPage>();
        Application.Current!.Windows[0].Page = new NavigationPage(loginPage);
    }
}

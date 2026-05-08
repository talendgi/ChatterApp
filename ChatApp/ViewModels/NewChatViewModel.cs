namespace ChatApp.ViewModels;

public partial class NewChatViewModel(Services.Abstractions.IChatService chatService) : ViewModels.Base.BaseViewModel
{
    private readonly Services.Abstractions.IChatService _chatService = chatService;

    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private string phoneNumber = string.Empty;

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private async Task StartChatAsync()
    {
        if (string.IsNullOrWhiteSpace(PhoneNumber))
        {
            await Shell.Current.DisplayAlertAsync("Invalid", "Enter a phone number.", "OK");
            return;
        }

        var chat = await _chatService.CreateOrGetDirectChatAsync(PhoneNumber);
        await Shell.Current.GoToAsync($"{nameof(Views.Pages.ChatDetailPage)}?chatThreadId={Uri.EscapeDataString(chat.Id)}");
    }
}

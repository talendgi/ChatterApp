using System.Collections.ObjectModel;
using ChatApp.Views.Pages;

namespace ChatApp.ViewModels;

public partial class ChatListViewModel(Services.Abstractions.IChatService chatService) : ViewModels.Base.BaseViewModel
{
    private readonly Services.Abstractions.IChatService _chatService = chatService;
    private readonly List<Models.ChatThread> _allChats = [];

    public ObservableCollection<Models.ChatThread> Chats { get; } = [];

    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private string searchText = string.Empty;

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            Chats.Clear();

            var items = await _chatService.GetChatThreadsAsync();
            _allChats.Clear();
            foreach (var item in items)
            {
                _allChats.Add(item);
            }

            ApplyFilter();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private async Task OpenChatAsync(Models.ChatThread? chat)
    {
        if (chat is null)
        {
            return;
        }

        var route = $"{nameof(ChatDetailPage)}?chatThreadId={Uri.EscapeDataString(chat.Id)}";
        await Shell.Current.GoToAsync(route);
    }

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private Task OpenSettingsAsync()
    {
        return Shell.Current.GoToAsync(nameof(Views.Pages.SettingsPage));
    }

    [CommunityToolkit.Mvvm.Input.RelayCommand]
    private Task OpenNewChatAsync()
    {
        return Shell.Current.GoToAsync(nameof(Views.Pages.NewChatPage));
    }

    partial void OnSearchTextChanged(string value)
    {
        ApplyFilter();
    }

    private void ApplyFilter()
    {
        Chats.Clear();

        var query = SearchText?.Trim();
        var filtered = string.IsNullOrWhiteSpace(query)
            ? _allChats
            : _allChats.Where(c => c.Title.Contains(query, StringComparison.OrdinalIgnoreCase)
                || c.LastMessagePreview.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();

        foreach (var item in filtered)
        {
            Chats.Add(item);
        }
    }
}

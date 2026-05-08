namespace ChatApp.Views.Pages;

public partial class ChatListPage : ContentPage
{
    public ChatListPage(ViewModels.ChatListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ViewModels.ChatListViewModel vm)
        {
            vm.LoadCommand.Execute(null);
        }
    }
}

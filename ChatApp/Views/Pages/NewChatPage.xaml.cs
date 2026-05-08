namespace ChatApp.Views.Pages;

public partial class NewChatPage : ContentPage
{
    public NewChatPage(ViewModels.NewChatViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

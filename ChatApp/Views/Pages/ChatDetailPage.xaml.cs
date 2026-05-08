namespace ChatApp.Views.Pages;

public partial class ChatDetailPage : ContentPage
{
    public ChatDetailPage(ViewModels.ChatDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ViewModels.ChatDetailViewModel vm)
        {
            vm.LoadCommand.Execute(null);
        }
    }
}

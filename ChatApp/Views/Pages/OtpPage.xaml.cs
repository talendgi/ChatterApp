namespace ChatApp.Views.Pages;

public partial class OtpPage : ContentPage
{
    public OtpPage(ViewModels.OtpViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

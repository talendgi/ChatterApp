namespace ChatApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(Views.Pages.ChatDetailPage), typeof(Views.Pages.ChatDetailPage));
        Routing.RegisterRoute(nameof(Views.Pages.SettingsPage), typeof(Views.Pages.SettingsPage));
        Routing.RegisterRoute(nameof(Views.Pages.NewChatPage), typeof(Views.Pages.NewChatPage));
        Routing.RegisterRoute(nameof(Views.Pages.OtpPage), typeof(Views.Pages.OtpPage));
    }
}

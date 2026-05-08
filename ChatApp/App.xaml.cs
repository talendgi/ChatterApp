using Microsoft.Extensions.DependencyInjection;

namespace ChatApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var serviceProvider = Handler?.MauiContext?.Services
            ?? IPlatformApplication.Current?.Services
            ?? throw new InvalidOperationException("Services are not available.");

        var loadingPage = new ContentPage
        {
            Content = new Grid
            {
                Children =
                {
                    new VerticalStackLayout
                    {
                        Spacing = 12,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        Children =
                        {
                            new ActivityIndicator { IsRunning = true, Color = Colors.Green, WidthRequest = 36, HeightRequest = 36 },
                            new Label { Text = "Loading ChatterBox...", HorizontalTextAlignment = TextAlignment.Center }
                        }
                    }
                }
            }
        };

        var window = new Window(loadingPage);
        _ = InitializeRootPageAsync(window, serviceProvider);
        return window;
    }

    private static async Task InitializeRootPageAsync(Window window, IServiceProvider serviceProvider)
    {
        var authService = serviceProvider.GetRequiredService<Services.Abstractions.IAuthService>();
        var isLoggedIn = await authService.IsLoggedInAsync();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            window.Page = isLoggedIn
                ? serviceProvider.GetRequiredService<AppShell>()
                : new NavigationPage(serviceProvider.GetRequiredService<Views.Pages.LoginPage>());
        });
    }
}

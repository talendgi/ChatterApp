using ChatApp.Data.Database;
using ChatApp.Data.Repositories;
using ChatApp.Services;
using ChatApp.Services.Abstractions;
using ChatApp.Services.Implementations;
using ChatApp.ViewModels;
using ChatApp.Views.Pages;
using Microsoft.Extensions.Logging;

namespace ChatApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        RegisterServices(builder.Services);
        RegisterViewModels(builder.Services);
        RegisterViews(builder.Services);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<DatabaseService>();
        services.AddSingleton<AppDatabase>();

        services.AddSingleton<IChatRepository, ChatRepository>();
        services.AddSingleton<IMessageRepository, MessageRepository>();

        services.AddSingleton<IChatService, ChatService>();
        services.AddSingleton<IMessageService, MessageService>();
        services.AddSingleton<IRealtimeService, RealtimeServiceStub>();
        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<IAppSettingsService, AppSettingsService>();
        services.AddSingleton<IAudioRecorderService, AudioRecorderService>();
    }

    private static void RegisterViewModels(IServiceCollection services)
    {
        services.AddTransient<ChatListViewModel>();
        services.AddTransient<ChatDetailViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<OtpViewModel>();
        services.AddTransient<SettingsViewModel>();
        services.AddTransient<NewChatViewModel>();
    }

    private static void RegisterViews(IServiceCollection services)
    {
        services.AddSingleton<AppShell>();
        services.AddTransient<ChatListPage>();
        services.AddTransient<ChatDetailPage>();
        services.AddTransient<StatusPage>();
        services.AddTransient<CallsPage>();
        services.AddTransient<LoginPage>();
        services.AddTransient<OtpPage>();
        services.AddTransient<SettingsPage>();
        services.AddTransient<NewChatPage>();
    }
}

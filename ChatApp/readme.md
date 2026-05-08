## .NET MAUI based chat application designed for scalable real-time messaging and modern UI experiences.
## Folder Structure

```text
YourApp/
  App.xaml
  AppShell.xaml
  MauiProgram.cs

  /Core
    /Constants
    /Enums
    /Helpers
    /Extensions

  /Models
    ChatThread.cs
    Message.cs
    UserProfile.cs

  /Data
    /Entities
      ChatThreadEntity.cs
      MessageEntity.cs
    /Database
      AppDatabase.cs
      DatabaseInitializer.cs
    /Repositories
      IChatRepository.cs
      IMessageRepository.cs
      ChatRepository.cs
      MessageRepository.cs

  /Services
    /Abstractions
      IChatService.cs
      IMessageService.cs
      IRealtimeService.cs
      INavigationService.cs
    /Implementations
      ChatService.cs
      MessageService.cs
      RealtimeServiceStub.cs
      NavigationService.cs

  /ViewModels
    /Base
      BaseViewModel.cs
    ChatListViewModel.cs
    ChatDetailViewModel.cs

  /Views
    /Pages
      ChatListPage.xaml
      ChatDetailPage.xaml
    /Controls
      MessageBubbleView.xaml
      EmptyStateView.xaml

  /DTOs
    MessageDto.cs
    ChatThreadDto.cs

  /Mapping
    EntityMapper.cs
    DtoMapper.cs

  /Resources
    /Styles
      Colors.xaml
      Typography.xaml
      Controls.xaml
      Themes.xaml
    /Images
    /Fonts
```

## Architecture (MVVM + Clean layering)

Use a layered, dependency-inverted flow:

`Views -> ViewModels -> Services (interfaces) -> Repositories -> SQLite`

- **Views**: XAML UI only, bindings and visual states.
- **ViewModels**: screen logic, commands, UI state, no database code.
- **Services**: app/business use-cases (send message, load chats, sync strategy).
- **Repositories**: persistence abstraction; hide SQLite details.
- **Data/Entities**: database schema-focused models.
- **Models/DTOs**: domain and transport models decoupled from DB structure.

For realtime-readiness, keep `IRealtimeService` as an interface now (stub implementation), so SignalR can be plugged in later without changing ViewModels.

##  NuGet Packages Needed

Core:
1. `CommunityToolkit.Mvvm` (MVVM boilerplate reduction)
2. `sqlite-net-pcl` (lightweight SQLite ORM)
3. `SQLitePCLRaw.bundle_e_sqlite3` (SQLite native bundle)

Helpful:
1. `CommunityToolkit.Maui` (UI helpers, converters, behaviors)
2. `Microsoft.Extensions.Logging.Debug` (debug logging)
3. `Polly` (retry/resilience for future network/realtime calls)

Future SignalR:
1. `Microsoft.AspNetCore.SignalR.Client`

## Responsibilities of Each Layer

- **Views**: Render chat list/detail, message bubbles, user interactions.
- **ViewModels**: Manage bindable state (`Chats`, `Messages`, `InputText`), execute commands (`SendMessageCommand`, `LoadChatsCommand`), trigger navigation.
- **Services**: Coordinate operations (validate message, timestamp, call repository, publish realtime event).
- **Repositories**: CRUD and query operations for chat threads/messages.
- **Database**: Table creation, indexing, connection lifecycle.
- **Models**: App domain meaning (thread/message semantics).
- **Entities**: SQLite storage shape and constraints.
- **Mapping**: Convert between Entity <-> Model <-> DTO cleanly.

### Demo video

https://drive.google.com/file/d/1c6bSHoxt64Kwk3dYJ6whbnULJ0svEzUT/view?usp=drive_link

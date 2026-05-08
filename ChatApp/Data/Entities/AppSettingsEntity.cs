namespace ChatApp.Data.Entities;

public class AppSettingsEntity
{
    [SQLite.PrimaryKey]
    public string Id { get; set; } = "app-settings";

    public string? ProfilePhotoPath { get; set; }
    public string? ChatBackgroundPath { get; set; }
}

namespace ChatApp.ViewModels.Base;

public abstract partial class BaseViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
{
    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private bool isBusy;

    [CommunityToolkit.Mvvm.ComponentModel.ObservableProperty]
    private string title = string.Empty;
}

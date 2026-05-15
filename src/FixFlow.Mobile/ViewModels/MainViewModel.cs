using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace FixFlow.Mobile.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string title = "FixFlow";

    [RelayCommand]
    void CambiarTexto()
    {
        Title = "holaaaaaaa";
    }
}
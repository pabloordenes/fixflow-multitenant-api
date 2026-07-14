using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using FixFlow.Mobile.Services;

namespace FixFlow.Mobile.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly AuthApiClient _authClient;

    [ObservableProperty] private string _email = string.Empty;

    [ObservableProperty] private string _password = string.Empty;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool _isBusy;

    [ObservableProperty] private string _errorMessage = string.Empty;
    public bool IsNotBusy => !IsBusy;

    [RelayCommand]
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Por favor, ingresa tus credenciales";
            return;
        }

        IsBusy = true;
        ErrorMessage = string.Empty;

        var response = await _authClient.LoginAsync(Email, Password);

        if (response != null && string.IsNullOrEmpty(response.Token))
        {
            // navegacion pendiente
            Console.WriteLine($"Token recibido:{response.Token}");
        }
        else
        {
            ErrorMessage = "Credenciales incorrectas o servidor no disponible";
        }

        IsBusy = false;
    }
}
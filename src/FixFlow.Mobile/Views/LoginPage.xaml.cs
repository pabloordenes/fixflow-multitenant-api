using FixFlow.Mobile.ViewModels;

namespace FixFlow.Mobile.Views;

public partial class LoginPage : ContentPage
{
    // El constructor recibe el ViewModel automáticamente gracias a la inyección de dependencias
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm; // Conectamos la Vista con su ViewModel
    }
}
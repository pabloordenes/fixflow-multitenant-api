using FixFlow.Mobile.ViewModels;

namespace FixFlow.Mobile.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
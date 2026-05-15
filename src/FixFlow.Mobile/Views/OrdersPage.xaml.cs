using FixFlow.Mobile.ViewModels;

namespace FixFlow.Mobile.Views;

public partial class OrdersPage : ContentPage
{
    public OrdersPage(OrdersViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
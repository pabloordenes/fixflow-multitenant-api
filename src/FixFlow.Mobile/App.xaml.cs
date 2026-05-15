using Microsoft.Extensions.DependencyInjection;

namespace FixFlow.Mobile;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // Esta es la forma moderna de arrancar MAUI sin warnings
        return new Window(new AppShell());
    }
}
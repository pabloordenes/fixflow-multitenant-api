using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using FixFlow.Mobile.ViewModels;
using FixFlow.Mobile.Services;
using FixFlow.Mobile.Views;

namespace FixFlow.Mobile;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				//fonts.AddFont("Livvic-Thin.ttf", "Livvic");
				//fonts.AddFont("MPLUS2-Bold.ttf", "MPLUS2Bold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddHttpClient<AuthApiClient>(client =>
		{
			client.BaseAddress = new Uri(AppConfig.DevTunnelBaseUrl);
			client.Timeout = TimeSpan.FromSeconds(30);
		});
		
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<LoginViewModel>();

		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<MainViewModel>();

		builder.Services.AddTransient<OrdersPage>();
		builder.Services.AddTransient<OrdersViewModel>();
		

		return builder.Build();
	}
}
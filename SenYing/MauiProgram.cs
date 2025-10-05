using Microsoft.Extensions.Logging;
using SenYing.Services;
using SenYing.Services.IServices;
using SenYing.ViewModels;

namespace SenYing;

public static class MauiProgram
{

	public static MauiApp CreateMauiApp()
	{
		
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddScoped<IM3u8Service, M3u8Service>();
		builder.Services.AddScoped<MainPageViewModel>();


#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

}

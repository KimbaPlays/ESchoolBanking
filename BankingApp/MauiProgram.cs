using SkiaSharp.Views.Maui.Controls.Hosting;
using CommunityToolkit.Maui;

namespace BankingApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSkiaSharp()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("App-Light.otf", "AgeoLight");
                fonts.AddFont("App-Normal.otf", "AgeoNormal");
                fonts.AddFont("App-Bold.otf", "AgeoBold");
            });

        return builder.Build();
    }
}

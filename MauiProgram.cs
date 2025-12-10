
namespace Scan2Cart;

public static class MauiProgram {
    public static MauiApp CreateMauiApp() {

        SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JFaF5cXGRCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWH1eeHRdQ2NZVUV3XkJWYEg=");

        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().ConfigureFonts(fonts => {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            fonts.AddFont("la-solid-900.ttf", "la");
        }).UseMauiCommunityToolkit().UseBarcodeReader().ConfigureSyncfusionCore().UseSkiaSharp();
#if DEBUG
        builder.Logging.AddDebug();
#endif
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<HomePageViewModel>();
        builder.Services.AddSingleton<IPageService, PageService>();
        builder.Services.AddSingleton<IDataProvider, DataProvider>();

        builder.Services.AddSingleton(provider => {
            return new FirebaseClient("https://snaplabel-88b46-default-rtdb.europe-west1.firebasedatabase.app/");
        });
        return builder.Build();
    }
}
using EconomizzeUserApp.Services.Components;
using EconomizzeUserApp.Services.Classes;
using Microsoft.Extensions.Logging;
using EconomizzeUserApp.Services.Interfaces;
using StoreApp.Services.Repositories;
using Blazored.Modal;
using System.Reflection;
using EconomizzeUserApp.Services.Classes.Handler;

namespace EconomizzeUserApp
{
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
                });

            builder.Services.AddSingleton(sp =>
            new GoogleCloudStorageService(
                bucketName: "economizze_user_app_test_storage_bucket",
                serviceAccountPath: Path.Combine(FileSystem.AppDataDirectory, "economizzeuserapp-441700-628b4df09d34.json")
                )
            );

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddBlazoredModal();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddSingleton<NavService>();
            builder.Services.AddSingleton<StatusHandler>();
            builder.Services.AddSingleton<UsernameHandler>();

            builder.Services.AddSingleton<SettingsService>();
            builder.Services.AddSingleton<IUserLoginService, UserLoginService>();
            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IStreetDetailViewService, StreetDetailViewService>();
            builder.Services.AddSingleton<IStreetService, StreetService>();
            builder.Services.AddSingleton<IUserAddressService, UserAddressService>();
            builder.Services.AddSingleton<IQuoteService, QuoteService>();
            builder.Services.AddSingleton<IPrescriptionService, PrescriptionService>();


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

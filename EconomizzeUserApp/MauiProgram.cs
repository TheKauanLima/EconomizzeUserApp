using EconomizzeUserApp.Services.Components;
using EconomizzeUserApp.Services.Classes;
using Microsoft.Extensions.Logging;
using EconomizzeUserApp.Services.Interfaces;
using StoreApp.Services.Repositories;
using Blazored.Modal;

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

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddBlazoredModal();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddSingleton<NavService>();
            builder.Services.AddSingleton<StatusHandler>();
            builder.Services.AddSingleton<UsernameHandler>();

            builder.Services.AddSingleton<SettingsService>();
            builder.Services.AddScoped<IUserLoginService, UserLoginService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IStreetDetailViewService, StreetDetailViewService>();
            builder.Services.AddScoped<IStreetService, StreetService>();
            builder.Services.AddScoped<IUserAddressService, UserAddressService>();
            builder.Services.AddScoped<IQuoteService, QuoteService>();
            builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

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

            //CopyServiceAccountKey();
            //VerifyServiceAccountKey();

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

        #region COPY AND VERIFY
        private static readonly ILogger logger = LoggerFactory.Create(builder => builder.AddDebug()).CreateLogger("AppLogger");

        //private static void CopyServiceAccountKey()
        //{
        //    try
        //    {
        //        // Debugging: List all embedded resources to confirm the correct name
        //        var assembly = Assembly.GetExecutingAssembly();
        //        var resourceNames = assembly.GetManifestResourceNames();
        //        foreach (var name in resourceNames)
        //        {
        //            logger.LogInformation($"Embedded Resource: {name}");
        //        }

        //        // Update with the correct resource name after verifying
        //        var resourceName = "EconomizzeUserApp.Resources.economizzeuserapp-441700-628b4df09d34.json";
        //        var destinationPath = Path.Combine(FileSystem.AppDataDirectory, "economizzeuserapp-441700-628b4df09d34.json");

        //        if (!File.Exists(destinationPath))
        //        {
        //            using Stream? resourceStream = assembly.GetManifestResourceStream(resourceName);
        //            if (resourceStream != null)
        //            {
        //                using FileStream destinationStream = File.Create(destinationPath);
        //                resourceStream.CopyTo(destinationStream);
        //                logger.LogInformation($"Service account file copied to {destinationPath}");
        //            }
        //            else
        //            {
        //                logger.LogError($"Error: Resource '{resourceName}' not found.");
        //            }
        //        }
        //        else
        //        {
        //            logger.LogError($"Service account file already exists at {destinationPath}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.LogError($"Error copying service account file: {ex.Message}");
        //    }
        //}


        //private static void VerifyServiceAccountKey()
        //{
        //    var serviceAccountPath = Path.Combine(FileSystem.AppDataDirectory, "economizzeuserapp-441700-628b4df09d34.json");

        //    if (File.Exists(serviceAccountPath))
        //    {
        //        logger.LogInformation($"Service account file found at {serviceAccountPath}");
        //    }
        //    else
        //    {
        //        logger.LogInformation($"Service account file NOT found at {serviceAccountPath}");
        //    }
        //}
        #endregion
    }
}

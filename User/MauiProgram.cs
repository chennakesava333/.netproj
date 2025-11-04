using Microsoft.Extensions.Logging;
using User.ViewModels;
using User.Views;

namespace User
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            //Views
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddTransient<RegistrationPage>();
            builder.Services.AddTransient<DashboardPage>();
            builder.Services.AddTransient<ServiceRequestPage>();
            builder.Services.AddTransient<NewInstallationPage>();
            builder.Services.AddTransient<ServiceUpgradePage>();


            //ViewModels
            builder.Services.AddSingleton<LoginPageViewModel>();
            builder.Services.AddTransient<RegistrationPageViewModel>();
            builder.Services.AddTransient<DashboardPageViewModel>();
            builder.Services.AddTransient<ServiceRequestPageViewModel>();
            builder.Services.AddTransient<NewInstallationViewModel>();
            builder.Services.AddTransient<ServiceUpgradePageViewModel>();


#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

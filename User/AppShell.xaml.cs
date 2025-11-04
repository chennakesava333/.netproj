using User.Views;

namespace User
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
            Routing.RegisterRoute(nameof(ServiceRequestPage), typeof(ServiceRequestPage));
            Routing.RegisterRoute(nameof(NewInstallationPage), typeof(NewInstallationPage));
            Routing.RegisterRoute(nameof(ServiceUpgradePage), typeof(ServiceUpgradePage));

        }
    }
}

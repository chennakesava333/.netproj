using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.ApplicationModel; // for MainThread
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using User.Models;
using User.Services;
using User.Views;

namespace User.ViewModels
{
    public partial class DashboardPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string displayName;

        [ObservableProperty]
        private ObservableCollection<NewInstallationModel> installations = new();

        [ObservableProperty]
        private bool isLoading;

        private readonly SQLiteHelper _dbHelper;

        // ✅ Use AsyncRelayCommand for async calls
        public IAsyncRelayCommand LoadInstallationsCommand { get; }

        public DashboardPageViewModel()
        {
            _dbHelper = new SQLiteHelper();
            LoadInstallationsCommand = new AsyncRelayCommand<string>(LoadInstallationsAsync);
        }

        // ✅ Public so DashboardPage.xaml.cs can call it
        public async Task LoadInstallationsAsync(string username)
        {
             username = Preferences.Get("LoggedInUsername", string.Empty);
            var data = await _dbHelper.GetInstallationsByUserAsync(username);

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Installations.Clear();
                foreach (var item in data)
                    Installations.Add(item);
            });
        }


        [RelayCommand]
        private async Task ImageClicked()
        {
            await Shell.Current.GoToAsync(nameof(ServiceRequestPage));
        }

        [RelayCommand]
        private async Task InstallationSelected(string installationId)
        {
            if (string.IsNullOrEmpty(installationId))
                return;

            // Navigate to NewInstallationPage with Id as query parameter
            await Shell.Current.GoToAsync($"{nameof(NewInstallationPage)}?Id={installationId}");
        }
    }
}

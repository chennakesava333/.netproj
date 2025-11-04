using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Views;

namespace User.ViewModels
{
    public partial class LoginPageViewModel : ObservableObject
    {

        private Services.SQLiteHelper dbHelper = new();

        [ObservableProperty]
        private string username = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string message;

        [RelayCommand]
        private async Task Login()
        {
            // ✅ Fix 1: Use properties with uppercase names (auto-generated)
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                Message = "Please enter both username and password.";
                return;
            }

            // ✅ Simple validation (replace with SQLite or API logic later)
            if (dbHelper.LoginUser(Username, Password))
            {
                Preferences.Set("LoggedInUsername", Username);
                // ✅ Fix 2: Use Shell navigation correctly and pass parameter
                await Shell.Current.GoToAsync($"{nameof(DashboardPage)}?UserName={Username}");

            }
            else
            {
                Message = "Invalid credentials!";
            }
        }

        [RelayCommand]
        private async Task GoToRegister()
        {
            await Shell.Current.GoToAsync(nameof(RegistrationPage));
        }

    }
}

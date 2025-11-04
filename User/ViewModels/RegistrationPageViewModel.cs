using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using User.Services;
using User.Views;

namespace User.ViewModels
{
    public partial class RegistrationPageViewModel : ObservableObject
    {
        private readonly SQLiteHelper _dbHelper;

        public RegistrationPageViewModel()
        {
            _dbHelper = new SQLiteHelper();
        }

        // Form Fields
        [ObservableProperty] private string name = string.Empty;
        [ObservableProperty] private string mobileNumber = string.Empty;
        [ObservableProperty] private string email = string.Empty;
        [ObservableProperty] private string password = string.Empty;
        [ObservableProperty] private string confirmPassword = string.Empty;
        [ObservableProperty] private string photoPath = string.Empty;

        // Validation Errors
        [ObservableProperty] private string? nameError;
        [ObservableProperty] private string? mobileError;
        [ObservableProperty] private string? emailError;
        [ObservableProperty] private string? passwordError;
        [ObservableProperty] private string? confirmPasswordError;

        // Register Command
        [RelayCommand]
        private async Task OnRegister()
        {
            // Clear previous errors
            NameError = MobileError = EmailError = PasswordError = ConfirmPasswordError = string.Empty;

            // Validation
            if (string.IsNullOrWhiteSpace(Name)) { NameError = "Name is required."; return; }
            if (string.IsNullOrWhiteSpace(MobileNumber)) { MobileError = "Mobile number is required."; return; }
            if (string.IsNullOrWhiteSpace(Email)) { EmailError = "Email is required."; return; }
            if (string.IsNullOrWhiteSpace(Password)) { PasswordError = "Password is required."; return; }
            if (Password != ConfirmPassword) { ConfirmPasswordError = "Passwords do not match."; return; }

            try
            {
                // Save to SQLite
                _dbHelper.RegisterUser(Name, MobileNumber, Email, Password);

                await Application.Current.MainPage.DisplayAlert("Success", "Registration successful!", "OK");

                // Navigate to Login page
                await Shell.Current.GoToAsync(nameof(LoginPage));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Registration failed: {ex.Message}", "OK");
            }
        }

        // Pick Photo Command
        [RelayCommand]
        public async Task PickPhotoAsync()
        {
            try
            {
                // Request permissions
                var cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
                var storageStatus = await Permissions.RequestAsync<Permissions.StorageRead>();

                if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
                {
                    await Application.Current.MainPage.DisplayAlert("Permission Denied", "Camera and storage permissions are required.", "OK");
                    return;
                }

                // Pick source
                string action = await Application.Current.MainPage.DisplayActionSheet("Select Photo", "Cancel", null, "Camera", "Gallery");

                FileResult? result = null;

                if (action == "Camera")
                    result = await MediaPicker.Default.CapturePhotoAsync();
                else if (action == "Gallery")
                    result = await MediaPicker.Default.PickPhotoAsync();

                if (result != null)
                {
                    var localFile = Path.Combine(FileSystem.AppDataDirectory, result.FileName);
                    using var stream = await result.OpenReadAsync();
                    using var fs = File.OpenWrite(localFile);
                    await stream.CopyToAsync(fs);

                    // Update bound property
                    PhotoPath = localFile;

                    await Application.Current.MainPage.DisplayAlert("Success", "Photo selected successfully!", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}

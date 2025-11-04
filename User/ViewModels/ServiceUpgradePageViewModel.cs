using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Services;

namespace User.ViewModels
{
    public partial class ServiceUpgradePageViewModel : ObservableObject
    {
        private readonly SQLiteHelper db;

        [ObservableProperty] private string customerName = string.Empty;
        [ObservableProperty] private string mobileNumber = string.Empty;
        [ObservableProperty] private string address = string.Empty;
        [ObservableProperty] private string existingSystemType = string.Empty;
        [ObservableProperty] private string existingCamerasText = string.Empty; // for Entry binding
        [ObservableProperty] private string newCamerasText = string.Empty;      // for Entry binding
        [ObservableProperty] private DateTime preferredUpgradeDate = DateTime.Today;
        [ObservableProperty] private string notes = string.Empty;

        // Error messages
        [ObservableProperty] private string customerNameError = string.Empty;
        [ObservableProperty] private string mobileNumberError = string.Empty;
        [ObservableProperty] private string existingSystemTypeError = string.Empty;
        [ObservableProperty] private string existingCamerasError = string.Empty;
        [ObservableProperty] private string newCamerasError = string.Empty;

        //public IAsyncRelayCommand SubmitCommand { get; }

        public ServiceUpgradePageViewModel()
        {
            db = new SQLiteHelper();
            //SubmitCommand = new AsyncRelayCommand(SubmitAsync);
        }

        [RelayCommand]
        private async Task Submit()
        {
            if (!Validate()) return;

            try
            {
                int existingCameras = int.Parse(existingCamerasText);
                int newCameras = int.Parse(newCamerasText);

                // Save to database
                db.ServiceUpgradeSystem(CustomerName, MobileNumber, Address, ExistingSystemType, existingCameras, newCameras, PreferredUpgradeDate, Notes);

                await Application.Current.MainPage.DisplayAlert("Success", "Upgrade record saved successfully!", "OK");

                ClearForm();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong: {ex.Message}", "OK");
            }
        }

        private bool Validate()
        {
            bool valid = true;

            CustomerNameError = string.IsNullOrWhiteSpace(CustomerName) ? "Customer Name is required" : string.Empty;
            MobileNumberError = string.IsNullOrWhiteSpace(MobileNumber) ? "Mobile Number is required" : string.Empty;
            ExistingSystemTypeError = string.IsNullOrWhiteSpace(ExistingSystemType) ? "Existing System Type is required" : string.Empty;

            // Validate cameras
            if (!int.TryParse(existingCamerasText, out _))
            {
                ExistingCamerasError = "Enter a valid number for Existing Cameras";
                valid = false;
            }
            else
            {
                ExistingCamerasError = string.Empty;
            }

            if (!int.TryParse(newCamerasText, out _))
            {
                NewCamerasError = "Enter a valid number for New Cameras";
                valid = false;
            }
            else
            {
                NewCamerasError = string.Empty;
            }

            if (!string.IsNullOrWhiteSpace(CustomerNameError)) valid = false;
            if (!string.IsNullOrWhiteSpace(MobileNumberError)) valid = false;
            if (!string.IsNullOrWhiteSpace(ExistingSystemTypeError)) valid = false;

            return valid;
        }

        private void ClearForm()
        {
            CustomerName = string.Empty;
            MobileNumber = string.Empty;
            Address = string.Empty;
            ExistingSystemType = string.Empty;
            ExistingCamerasText = string.Empty;
            NewCamerasText = string.Empty;
            PreferredUpgradeDate = DateTime.Today;
            Notes = string.Empty;
        }
    }
}

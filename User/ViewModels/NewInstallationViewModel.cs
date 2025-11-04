using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using User.Services;
using User.Views;

namespace User.ViewModels
{
    [QueryProperty(nameof(Id), "Id")]
    public partial class NewInstallationViewModel : ObservableObject
    {

        private readonly SQLiteHelper db;

        [ObservableProperty] private string id;

        // ------------------ Customer Details ------------------
        [ObservableProperty]
        private string userName;
        [ObservableProperty] private string customerName;
        [ObservableProperty] private string contactNumber;
        [ObservableProperty] private string email;
        [ObservableProperty] private DateTime installDate = DateTime.Now;

        // ------------------ Address Section ------------------
        [ObservableProperty] private string pincode;
        [ObservableProperty] private string locality;
        [ObservableProperty] private string address;
        [ObservableProperty] private string city;
        [ObservableProperty] private string selectedState;
        [ObservableProperty] private string landmark;
        [ObservableProperty] private string alternatePhone;
        [ObservableProperty] private bool isHome = true;
        [ObservableProperty] private bool isWork = false;

        public ObservableCollection<string> States { get; } = new()
        {
            "Andhra Pradesh", "Telangana", "Tamil Nadu", "Karnataka", "Kerala",
            "Maharashtra", "Gujarat", "Delhi", "Rajasthan", "Uttar Pradesh"
        };

        // ------------------ System Details ------------------
        public ObservableCollection<SystemItem> SystemDetails { get; set; } = new();

        [RelayCommand]
        private void AddSystemItem()
        {
            SystemDetails.Add(new SystemItem());
        }

        // ------------------ Camera Placement ------------------
        public ObservableCollection<CameraPlacement> CameraPlacements { get; set; } = new();

        [RelayCommand]
        private void AddCameraPlacement()
        {
            var newCamNo = CameraPlacements.Count + 1;
            CameraPlacements.Add(new CameraPlacement
            {
                CameraNo = $"Cam {newCamNo}"
            });
        }

        // ------------------ Network Configuration ------------------
        [ObservableProperty] private bool internetConnected;
        [ObservableProperty] private bool mobileAppConfigured;
        [ObservableProperty] private string appName;
        [ObservableProperty] private bool viewOnAndroid;
        [ObservableProperty] private bool viewOniPhone;
        [ObservableProperty] private bool viewOnDesktop;

        // ------------------ Acknowledgment ------------------
        [ObservableProperty] private bool installedChecked;
        [ObservableProperty] private bool demoReceived;
        [ObservableProperty] private bool understoodOperation;

        // ------------------ Signatures ------------------
        [ObservableProperty] private string customerSign;
        [ObservableProperty] private string technicianSign;

        // ------------------ Commands ------------------
        [RelayCommand]
        private async Task GetCurrentLocationAsync()
        {
            try
            {
                var location = await Geolocation.Default.GetLocationAsync(
                    new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10))
                );

                if (location == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Unable to fetch location", "OK");
                    return;
                }

                // Convert coordinates into human-readable address (reverse geocoding)
                var placemarks = await Geocoding.Default.GetPlacemarksAsync(location);
                var placemark = placemarks?.FirstOrDefault();

                if (placemark != null)
                {
                    Address = $"{placemark.Thoroughfare} {placemark.SubThoroughfare}".Trim();
                    City = placemark.Locality;
                    SelectedState = placemark.AdminArea;
                    Pincode = placemark.PostalCode;
                    Locality = placemark.SubLocality;
                }


                await Application.Current.MainPage.DisplayAlert("Success", "Address autofilled successfully!", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Location Error", ex.Message, "OK");
            }
        }

        [RelayCommand]
        private async Task SubmitInstallation()
        {
            // Basic Validation
            if (string.IsNullOrWhiteSpace(CustomerName) || string.IsNullOrWhiteSpace(ContactNumber))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please fill in Customer Name and Contact Number.", "OK");
                return;
            }

            // In real app: save data to DB
            if (string.IsNullOrEmpty(Id))
            {
                db.newInstallation(
               CustomerName ?? string.Empty,
               ContactNumber ?? string.Empty,
               Email ?? string.Empty,
               InstallDate,
               Pincode ?? string.Empty,
               Locality ?? string.Empty,
               Address ?? string.Empty,
               City ?? string.Empty,
               SelectedState ?? string.Empty,
               Landmark ?? string.Empty,
               AlternatePhone ?? string.Empty,
               IsHome,
               IsWork,
               InternetConnected,
               MobileAppConfigured,
               AppName ?? string.Empty,
               ViewOnAndroid,
               ViewOniPhone,
               ViewOnDesktop,
               InstalledChecked,
               DemoReceived,
               UnderstoodOperation,
               CustomerSign ?? string.Empty,
               TechnicianSign ?? string.Empty
               );
                await Application.Current.MainPage.DisplayAlert("Success", "Installation details saved successfully.", "OK");

                await Shell.Current.GoToAsync(nameof(DashboardPage));
            }
            else
            {
                // UPDATE EXISTING RECORD
                await db.UpdateInstallationAsync(
                    Id, CustomerName, ContactNumber, Email, InstallDate,
                    Pincode, Locality, Address, City, SelectedState, Landmark,
                    AlternatePhone, IsHome, IsWork, InternetConnected, MobileAppConfigured,
                    AppName, ViewOnAndroid, ViewOniPhone, ViewOnDesktop,
                    InstalledChecked, DemoReceived, UnderstoodOperation,
                    CustomerSign, TechnicianSign
                );

                await Application.Current.MainPage.DisplayAlert("✅ Success", "Installation record updated successfully.", "OK");
            }

            await Shell.Current.GoToAsync(nameof(DashboardPage));
        }
        

        // ------------------ Constructor ------------------

       
        public NewInstallationViewModel()
        {
            AddSystemItem();
            AddCameraPlacement();
            db = new SQLiteHelper();

            
        }

        // Update only changed fields
        public async void OnNavigatedTo()
        {
            if (string.IsNullOrEmpty(id))
                return;

            var record = await db.GetInstallationByIdAsync(id);
            if (record != null)
            {
                UserName = record.UserName;
                CustomerName = record.CustomerName;
                ContactNumber = record.ContactNumber;
                Email = record.Email;
                InstallDate = DateTime.TryParse(record.InstallDate, out var parsed) ? parsed : DateTime.Now;

                Pincode = record.Pincode;
                Locality = record.Locality;
                Address = record.Address;
                City = record.City;
                SelectedState = record.SelectedState;
                Landmark = record.Landmark;
                AlternatePhone = record.AlternatePhone;
                IsHome = record.IsHome;
                IsWork = record.IsWork;

                InternetConnected = record.InternetConnected;
                MobileAppConfigured = record.MobileAppConfigured;
                AppName = record.AppName;
                ViewOnAndroid = record.ViewOnAndroid;
                ViewOniPhone = record.ViewOniPhone;
                ViewOnDesktop = record.ViewOnDesktop;

                InstalledChecked = record.InstalledChecked;
                DemoReceived = record.DemoReceived;
                UnderstoodOperation = record.UnderstoodOperation;

                CustomerSign = record.CustomerSign;
                TechnicianSign = record.TechnicianSign;
            }
        }
    }

    // ====================== Helper Models ======================
    public class SystemItem
    {
        public string ItemName { get; set; }
        public string Quantity { get; set; }
        public string BrandModel { get; set; }
        public string SerialNumber { get; set; }
    }

    public class CameraPlacement
    {
        public string CameraNo { get; set; }
        public string LocationDescription { get; set; }
        public string IndoorOutdoor { get; set; }
        public string Type { get; set; }

        public ObservableCollection<string> IndoorOutdoorOptions { get; } = new()
        {
            "Indoor", "Outdoor"
        };

        public ObservableCollection<string> TypeOptions { get; } = new()
        {
            "Bullet", "Dome", "PTZ", "Other"
        };
    }


}

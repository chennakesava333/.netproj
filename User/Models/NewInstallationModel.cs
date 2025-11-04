using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Models
{
    public class NewInstallationModel
    {
        public string Id { get; set; }
        public string UserName { get; set; } = string.Empty;

        // Customer Details
        public string CustomerName { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string InstallDate { get; set; }

        // Address Section
        public string Pincode { get; set; }
        public string Locality { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string SelectedState { get; set; }
        public string Landmark { get; set; }
        public string AlternatePhone { get; set; }
        public bool IsHome { get; set; }
        public bool IsWork { get; set; }

        // Network Configuration
        public bool InternetConnected { get; set; }
        public bool MobileAppConfigured { get; set; }
        public string AppName { get; set; }
        public bool ViewOnAndroid { get; set; }
        public bool ViewOniPhone { get; set; }
        public bool ViewOnDesktop { get; set; }

        // Acknowledgment
        public bool InstalledChecked { get; set; }
        public bool DemoReceived { get; set; }
        public bool UnderstoodOperation { get; set; }

        // Signatures
        public string CustomerSign { get; set; }
        public string TechnicianSign { get; set; }


    }
}

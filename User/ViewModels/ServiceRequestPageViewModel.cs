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
    public partial class ServiceRequestPageViewModel : ObservableObject
    {
        [RelayCommand]
        private async Task NewInstallation()
        {
            await Shell.Current.GoToAsync(nameof(NewInstallationPage));
        }
        [RelayCommand]
        private async Task ServiceUpgrade()
        {
            await Shell.Current.GoToAsync(nameof(ServiceUpgradePage));
        }
    }
}

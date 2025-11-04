using User.ViewModels;

namespace User.Views;

public partial class NewInstallationPage : ContentPage
{
    private NewInstallationViewModel ViewModel => BindingContext as NewInstallationViewModel;

    public NewInstallationPage()
    {
        InitializeComponent();
        BindingContext = new NewInstallationViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ViewModel?.OnNavigatedTo(); // Prefill from DB when navigated with ID
    }
}             
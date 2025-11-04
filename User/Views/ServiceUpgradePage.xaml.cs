using User.ViewModels;

namespace User.Views;

public partial class ServiceUpgradePage : ContentPage
{
	public ServiceUpgradePage( ServiceUpgradePageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
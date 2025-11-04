using User.ViewModels;

namespace User.Views;

public partial class RegistrationPage : ContentPage
{
	public RegistrationPage(RegistrationPageViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}
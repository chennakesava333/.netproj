using User.ViewModels;

namespace User.Views;

[QueryProperty(nameof(UserName), "UserName")]
public partial class DashboardPage : ContentPage
{

    private readonly DashboardPageViewModel _viewModel;
    private string userName;
   
       public string UserName
    {
        get => _viewModel.DisplayName;
        set => _viewModel.DisplayName = $"Welcome, {value}";
    }
    
    public DashboardPage(DashboardPageViewModel vm)
	{
		InitializeComponent();
        BindingContext = _viewModel = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadInstallationsCommand.CanExecute(null))
            await _viewModel.LoadInstallationsCommand.ExecuteAsync(null);
    }

}
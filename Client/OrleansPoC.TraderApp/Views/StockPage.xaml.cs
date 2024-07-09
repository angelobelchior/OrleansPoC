namespace OrleansPoC.TraderApp.Views;

public partial class StockPage : ContentPage
{
    private readonly ViewModels.StockViewModel _viewModel;
    public StockPage(Models.Customer customer)
    {
        InitializeComponent();
        BindingContext = _viewModel = new ViewModels.StockViewModel(customer);;
    }

    protected override void OnAppearing()
    {
        _viewModel.OnInitializing();
        base.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        _viewModel.OnDisposing();
        base.OnDisappearing();
    }
}
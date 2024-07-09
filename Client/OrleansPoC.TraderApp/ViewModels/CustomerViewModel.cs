using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OrleansPoC.TraderApp.Services;
using OrleansPoC.TraderApp.Views;

namespace OrleansPoC.TraderApp.ViewModels;

public partial class CustomerViewModel : ObservableObject
{
    [ObservableProperty]
    private string _name = "Angelo";
    
    [ObservableProperty]
    private string _stocks = "PETR3,JBSS3,ITUB4,VALE3,IRBR3,AZUL4";

    [RelayCommand]
    private async Task CreateCustomerAsync()
    {
        var service = new CustomerService(MauiProgram.BaseAddress);
        var customer = await service.CreateCustomerAsync(Name, Stocks.Split(','));
        await Application.Current!.MainPage!.Navigation.PushAsync(new StockPage(customer));
    }
}
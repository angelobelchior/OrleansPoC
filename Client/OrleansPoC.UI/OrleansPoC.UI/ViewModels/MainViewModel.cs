using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OrleansPoC.Contracts.Models;
using OrleansPoC.UI.Models;
using OrleansPoC.UI.Views;

namespace OrleansPoC.UI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] public string _endpoint = "http://localhost:5070/";
    [ObservableProperty] public string _name = string.Empty;
    [ObservableProperty] public StockItem[] _stocks;

    public MainViewModel()
    {
        _stocks = Stock.GetStockList().Select(s => new StockItem
        {
            Name = s,
            IsChecked = false
        }).ToArray();
    }

    [RelayCommand]
    private async Task SendAsync()
    {
        var customer = await CreateCustomerAsync().ConfigureAwait(true);;
        ShowStocks(customer);
        Clear();
    }

    private async Task<Customer> CreateCustomerAsync()
    {
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(Endpoint);
        var customer = new Customer
        {
            Name = Name,
            Stocks = Stocks.Where(s => s.IsChecked).Select(s => s.Name).ToArray()
        };
        var response = await httpClient.PostAsJsonAsync("customers", customer).ConfigureAwait(true);
        return await response.Content.ReadFromJsonAsync<Customer>() ?? throw new InvalidOperationException();
    }
    
    private void ShowStocks(Customer customer)
    {
        var stocksViewModel = new StocksViewModel
        {
            Customer = customer,
            Endpoint = Endpoint
        };
        var stocksWindow = new Window
        {
            Title = customer.Name,
            Width = 200,
            Height = 300,
            DataContext = stocksViewModel,
            Content = new StocksView
            {
                ViewModel = stocksViewModel
            }
        };
        stocksWindow.Show();
    }
    
    private void Clear()
    {
        Name = string.Empty;
        foreach (var stock in Stocks)
            stock.IsChecked = false;
    }
}
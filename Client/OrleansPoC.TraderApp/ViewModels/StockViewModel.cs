using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using OrleansPoC.TraderApp.Models;
using OrleansPoC.TraderApp.Services;

namespace OrleansPoC.TraderApp.ViewModels;

public partial class StockViewModel : ObservableObject
{
    public ObservableCollection<Stock> Items { get; set; }

    private CustomerService? _service;
    private readonly Customer _customer;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public StockViewModel(Customer customer)
    {
        _customer = customer;
        _service = new CustomerService(MauiProgram.BaseAddress);

        Items = new ObservableCollection<Stock>();
        foreach (var stock in _customer.Stocks)
            Items.Add(new Stock { Name = stock });

        WeakReferenceMessenger.Default.Register<StockChangedMessage>(this, (_, message) => StockChanged(message.Value));
    }
    
    private void StockChanged(Stock stock)
    {
        if (Items.Any(i => i.Name == stock.Name))
        {
            var existingStock = Items.First(s => s.Name == stock.Name);
            
            if(stock.Value == existingStock.Value)
                existingStock.VariationIcon = "–";
            
            if(stock.Value > existingStock.Value)
                existingStock.VariationIcon = "↑";
            
            if(stock.Value < existingStock.Value)
                existingStock.VariationIcon = "↓";
            
            existingStock.Value = stock.Value;
        }
        else
        {
            Items.Add(stock);
        }
    }
    
    public void OnInitializing()
    {
        Task.Run(() => _service!.BeginReceivingStocksAsync(_customer.Id, _cancellationTokenSource.Token));
    }

    public void OnDisposing()
    {
        _cancellationTokenSource.Cancel();
        WeakReferenceMessenger.Default.UnregisterAll(this);
        _service = null;
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }
}
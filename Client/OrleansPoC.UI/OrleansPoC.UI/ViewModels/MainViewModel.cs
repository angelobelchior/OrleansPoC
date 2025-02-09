namespace OrleansPoC.UI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private string _endpoint = "http://localhost:5070/";
    [ObservableProperty] private StockItem[] _stocks;

    public MainViewModel()
    {
        _stocks = Stock.GetStockNameList().Select(s => new StockItem
        {
            Name = s,
            IsChecked = false
        }).ToArray();
    }

    [RelayCommand]
    private void OpenBooks()
    {
        foreach (var stock in Stocks.Where(s => s.IsChecked))
        {
            var bookViewModel = new BookViewModel
            {
                StockName = stock.Name,
                Endpoint = Endpoint
            };
            var stocksWindow = new Window
            {
                Title = stock.Name,
                Width = 500,
                Height = 600,
                DataContext = bookViewModel,
                CanResize = false,
                ShowActivated = true,
                WindowState = WindowState.Normal,
                WindowStartupLocation = WindowStartupLocation.Manual,
                Content = new BookView
                {
                    ViewModel = bookViewModel
                }
            };
            stocksWindow.Show();
        }

        Clear();
    }

    private void Clear()
    {
        foreach (var stock in Stocks)
            stock.IsChecked = false;
    }
}
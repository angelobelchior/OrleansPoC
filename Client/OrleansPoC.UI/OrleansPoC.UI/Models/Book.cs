namespace OrleansPoC.UI.Models;

public partial class Book(string name) : ObservableObject
{
    public static readonly Book Empty = new(string.Empty);

    public string Name { get; } = name;

    [ObservableProperty] private decimal _lastPrice;
    [ObservableProperty] private decimal _variation;
    [ObservableProperty] private DateTime _dateTime;
    [ObservableProperty] private long _volume;
    [ObservableProperty] private long _trades;
    [ObservableProperty] private decimal _high;
    [ObservableProperty] private decimal _low;

    public LimitedObservableCollection<Stock> Buy { get; set; } = new(12);
    public LimitedObservableCollection<Stock> Sell { get; set; } = new(12);


    public void AddStock(Stock stock)
    {
        if (stock.TransactionType == TransactionType.Buy)
            Buy.Add(stock);
        else
            Sell.Add(stock);

        DateTime = stock.DateTime;
        Variation = LastPrice == 0 ? 0 : ((stock.Value - LastPrice) / LastPrice) * 100m;
        Volume += stock.Volume;
        Trades += Buy.Count + Sell.Count;
        if (High < stock.Value) High = stock.Value;
        if (Low > stock.Value) Low = stock.Value;
        LastPrice = stock.Value;
    }

    public static bool IsEmpty(Book book) => book.Name == Empty.Name;

    public static Book Create(Stock stock)
    {
        var book = new Book(stock.Name)
        {
            LastPrice = stock.Value,
            DateTime = stock.DateTime,
            High = stock.Value,
            Low = stock.Value
        };
        return book;
    }
}
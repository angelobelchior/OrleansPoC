namespace OrleansPoC.UI.ViewModels;

public partial class BookViewModel : ObservableObject
{
    public required string StockName { get; init; }
    public required string Endpoint { get; init; }
    public Func<Task>? OnInitializedAction { get; set; }
    public Func<Task>? OnFinalizedAction { get; set; }


    [ObservableProperty] private Book _book = Book.Empty;

    private readonly CancellationTokenSource _cancellationTokenSource;

    public BookViewModel()
    {
        _cancellationTokenSource = new CancellationTokenSource();

        OnInitializedAction = OnInitialized;
        OnFinalizedAction = OnFinalized;
    }

    private async Task OnInitialized()
    {
        try
        {
            var httpClient = new HttpClient(new SocketsHttpHandler
            {
                EnableMultipleHttp3Connections = true
            })
            {
                BaseAddress = new Uri(Endpoint),
            };
            var url = $"stocks/{StockName}/book";
            await using var stream =
                await httpClient.GetStreamAsync(url, _cancellationTokenSource.Token).ConfigureAwait(true);
            await foreach (var item in SseParser.Create(stream).EnumerateAsync(_cancellationTokenSource.Token)
                               .ConfigureAwait(true))
            {
                if (item.EventType != "stockChanged") continue;

                var stock = JsonSerializer.Deserialize<Stock>(item.Data);
                if (stock is null) continue;

                if (Book.IsEmpty(Book)) Book = Book.Create(stock);
                Book.AddStock(stock);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task OnFinalized()
    {
        OnInitializedAction = null;
        OnInitializedAction = null;
        await _cancellationTokenSource.CancelAsync().ConfigureAwait(true);
    }
}
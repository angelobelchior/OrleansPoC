namespace OrleansPoC.UI.Views;

public partial class BookView : UserControl
{
    public BookViewModel? ViewModel { get; set; }

    public BookView()
    {
        InitializeComponent();
    }

    protected override async void OnLoaded(RoutedEventArgs e)
    {
        DataContext = ViewModel;
        
        if (ViewModel?.OnInitializedAction is not null)
            await ViewModel.OnInitializedAction.Invoke().ConfigureAwait(true);;

        base.OnLoaded(e);
    }

    protected override async void OnUnloaded(RoutedEventArgs e)
    {
        if (ViewModel?.OnFinalizedAction is not null)
            await ViewModel.OnFinalizedAction().ConfigureAwait(true);;
        base.OnUnloaded(e);
    }
}
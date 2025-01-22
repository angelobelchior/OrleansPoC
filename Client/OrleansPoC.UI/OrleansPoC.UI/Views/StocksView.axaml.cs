using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using OrleansPoC.UI.ViewModels;

namespace OrleansPoC.UI.Views;

public partial class StocksView : UserControl
{
    public StocksViewModel? ViewModel { get; set; }

    public StocksView()
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
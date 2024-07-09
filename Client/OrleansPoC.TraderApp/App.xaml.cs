using OrleansPoC.TraderApp.Views;

namespace OrleansPoC.TraderApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new CustomerPage());
    }
}
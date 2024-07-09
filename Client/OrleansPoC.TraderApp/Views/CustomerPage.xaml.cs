using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrleansPoC.TraderApp.ViewModels;

namespace OrleansPoC.TraderApp.Views;

public partial class CustomerPage : ContentPage
{
    public CustomerPage()
    {
        InitializeComponent();
        BindingContext = new CustomerViewModel();
    }
}
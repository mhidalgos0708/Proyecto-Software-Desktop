using Excalinest.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Excalinest.Views;

public sealed partial class VideogamesPage : Page
{
    public VideogamesViewModel ViewModel
    {
        get;
    }

    public VideogamesPage()
    {
        ViewModel = App.GetService<VideogamesViewModel>();
        InitializeComponent();
    }
}

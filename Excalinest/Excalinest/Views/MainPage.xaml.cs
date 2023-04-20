using System.Diagnostics;
using Excalinest.Core.Models;
using Excalinest.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Excalinest.Views;

public sealed partial class MainPage : Page
{
    public BitmapImage cover;
    public StackPanel myStackPanel;

    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();

        /*if (GridView.DataFetchSize ==0)
        {
            gamesOrNot.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
        }*/
        // taglist.ItemsSource = ViewModel.Tags;
    }
    public async void TagComboBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        ComboBox tag = (ComboBox)sender;
        Tag chosenTag = tag.SelectedItem as Tag;

        await ViewModel.GetVideojuegosByTag(chosenTag.ID);
    }


}
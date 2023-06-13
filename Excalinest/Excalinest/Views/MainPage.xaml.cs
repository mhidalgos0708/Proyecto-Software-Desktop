using System.Diagnostics;
using Excalinest.Core.Models;
using Excalinest.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml;
using Excalinest.Strings;

namespace Excalinest.Views;

public sealed partial class MainPage : Page
{
    public BitmapImage cover;
    public StackPanel myStackPanel;

    private GlobalFunctions _globalFunctions;
    public MainViewModel ViewModel
    {
        get;
    }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        InitializeComponent();

        _globalFunctions = new GlobalFunctions();
    }
    public async void TagComboBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        if (_globalFunctions.CheckInternetConnectivity())
        {
            ComboBox tag = (ComboBox)sender;
            if (tag.SelectedItem is Tag chosenTag)
            {
                try
                {
                    await ViewModel.GetVideojuegosByTag(chosenTag.ID);
                }
                catch (Exception ex)
                {
                    ContentDialog dialog = new ContentDialog();
                    dialog.XamlRoot = this.XamlRoot;
                    dialog.Style = Microsoft.UI.Xaml.Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                    dialog.Title = "Atención";
                    dialog.PrimaryButtonText = "Ok";
                    dialog.DefaultButton = ContentDialogButton.Primary;

                    var message = "Error: " + ex;
                    dialog.Content = new Dialog(message);
                    await dialog.ShowAsync();
                }
            }
        }
        
    }

}
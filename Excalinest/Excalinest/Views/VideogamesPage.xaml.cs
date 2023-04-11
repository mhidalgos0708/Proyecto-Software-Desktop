using System.Diagnostics;
using Excalinest.Core.Models;
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

    public async void TagComboBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        ComboBox tag = (ComboBox) sender;
        if (tag.SelectedItem is Tag chosenTag)
        {
            await ViewModel.GetVideojuegosByTag(chosenTag.ID);
        }
    }
}
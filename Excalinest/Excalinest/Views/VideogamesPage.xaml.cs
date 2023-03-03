using System.Diagnostics;
using Excalinest.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.VoiceCommands;
using Windows.UI.Popups;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using System;

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

    public void TagComboBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        ComboBox tag = (ComboBox)sender;
        String chosenTag = tag.SelectedItem as String;
        Debug.WriteLine(chosenTag);

       
    }
}

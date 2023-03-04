using System.Diagnostics;
using Excalinest.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.VoiceCommands;
using Windows.UI.Popups;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using System;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.InteropServices;

namespace Excalinest.Views;

public sealed partial class VideogamesPage : Page
{
    public BitmapImage cover;

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

using System.Diagnostics;
using Excalinest.Core.Models;
using Excalinest.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;

namespace Excalinest.Views;

public sealed partial class VideogamesPage : Page
{
    public BitmapImage cover;
    public StackPanel myStackPanel;

    public VideogamesViewModel ViewModel
    {
        get;
    }

    public VideogamesPage()
    {
        ViewModel = App.GetService<VideogamesViewModel>();
        InitializeComponent();

        // taglist.ItemsSource = ViewModel.Tags;
    }



    /*public async void GetPortadas()
    {
        List<BitmapImage> images = new List<BitmapImage>();
        // Assuming you have a list of MongoDB documents named "documents" containing image data as a byte array
        var imageControls = new List<Image>();

        foreach (var document in ViewModel.Source)
        {
            using (var memoryStream = new MemoryStream(document.Portada.Data))
            {
                var bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(memoryStream.AsRandomAccessStream());

                var imageControl = new Image();
                imageControl.Source = bitmapImage;

                imageControls.Add(imageControl);
                images.Add(bitmapImage);
            }
        }

        // Add the image controls to your UI as needed
        foreach (var imageControl in imageControls)
        {
            myStackPanel.Children.Add(imageControl); // Assumes you have a StackPanel named "myStackPanel" in your UI
            
        }
    }*/

    public async void TagComboBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        ComboBox tag = (ComboBox)sender;
        Tag chosenTag = tag.SelectedItem as Tag;
        Debug.WriteLine(chosenTag.ID);
        await ViewModel.GetVideojuegosByTag(chosenTag.ID);
    }

    
}
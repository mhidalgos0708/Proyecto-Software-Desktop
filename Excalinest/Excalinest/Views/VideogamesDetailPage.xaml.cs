using System.Drawing;
using CommunityToolkit.WinUI.UI.Animations;

using Excalinest.Contracts.Services;
using Excalinest.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;

using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using Microsoft.AspNetCore.Mvc;
using Image = System.Drawing.Image;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Streams;
using Microsoft.UI.Xaml;
using System.Diagnostics;
using System.Security.Policy;

namespace Excalinest.Views;

public sealed partial class VideogamesDetailPage : Page
{

    public VideogamesDetailViewModel ViewModel
    {
        get;
    }

 

    public VideogamesDetailPage()
    {
        ViewModel = App.GetService<VideogamesDetailViewModel>();
        InitializeComponent();

        List<string> items = new List<string>();
        items.Add("2D");
        items.Add("Adventure");

        tagsList.ItemsSource = items;

    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        this.RegisterElementForConnectedAnimation("animationKeyContentGrid", itemHero);

        GetImage();

    }

    private async void GetImage()
    {
        BitmapImage biSource = new BitmapImage();
        using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
        {
            await stream.WriteAsync(ViewModel.Item.Portada.Data.AsBuffer());
            stream.Seek(0);
            await biSource.SetSourceAsync(stream);
        }

        Cover.Source = biSource;
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
        if (e.NavigationMode == NavigationMode.Back)
        {
            var navigationService = App.GetService<INavigationService>();

            if (ViewModel.Item != null)
            {
                navigationService.SetListDataItemForNextConnectedAnimation(ViewModel.Item);
                
            }
        }
    }

    
}

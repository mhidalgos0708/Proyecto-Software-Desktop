using CommunityToolkit.WinUI.UI.Animations;

using Excalinest.Contracts.Services;
using Excalinest.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Image = Microsoft.UI.Xaml.Controls.Image;

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
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        this.RegisterElementForConnectedAnimation("animationKeyContentGrid", itemHero);

        GetImageCover();
        GetImageFacebook();
        GetImageInstagram();
        GetImageTwitter();

        if (ViewModel.Item != null)
        {
            tagsList.ItemsSource = ViewModel.Item.Etiquetas;
        }
        
        var downloadGroup = FindName("downloadGroup") as StackPanel;
        var executeGroup = FindName("executeGroup") as StackPanel;

        if (VideogamesDetailViewModel.EsVideojuegoDescargado())
        {
            if (downloadGroup != null)
            {
                downloadGroup.Visibility = Visibility.Collapsed;
            }
        }
        else
        {
            if (executeGroup != null)
            {
                executeGroup.Visibility = Visibility.Collapsed;
            }
        }
    }

    private async void GetImageCover()
    {
        if (ViewModel.Item != null)
        {
            using var memoryStream = new MemoryStream(ViewModel.Item.Portada.Data);
            var bitmapImage = new BitmapImage();
            await bitmapImage.SetSourceAsync(memoryStream.AsRandomAccessStream());

            var imageControl = new Image();
            Cover.Source = bitmapImage;
        }
    }

    private async void GetImageTwitter()
    {
        if (ViewModel.Item != null)
        {
            using var memoryStream = new MemoryStream(ViewModel.Item.Twitter.Data);
            var bitmapImage = new BitmapImage();
            await bitmapImage.SetSourceAsync(memoryStream.AsRandomAccessStream());

            var imageControl = new Image();
            Twitter.Source = bitmapImage;
        }
    }

    private async void GetImageFacebook()
    {
        if (ViewModel.Item != null)
        {
            using var memoryStream = new MemoryStream(ViewModel.Item.Facebook.Data);
            var bitmapImage = new BitmapImage();
            await bitmapImage.SetSourceAsync(memoryStream.AsRandomAccessStream());

            var imageControl = new Image();
            Facebook.Source = bitmapImage;
        }
    }

    private async void GetImageInstagram()
    {
        if (ViewModel.Item != null)
        {
            using var memoryStream = new MemoryStream(ViewModel.Item.Instagram.Data);
            var bitmapImage = new BitmapImage();
            await bitmapImage.SetSourceAsync(memoryStream.AsRandomAccessStream());

            var imageControl = new Image();
            Instagram.Source = bitmapImage;
        }
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

    private void EjecutarVideojuego(object sender, RoutedEventArgs e)
    {
        VideogamesDetailViewModel.EjecutarVideojuego();
    }

    private void EliminarVideojuego(object sender, RoutedEventArgs e)
    {
        VideogamesDetailViewModel.EliminarVideojuego();

        var downloadGroup = FindName("downloadGroup") as StackPanel;
        var executeGroup = FindName("executeGroup") as StackPanel;

        if (downloadGroup != null && executeGroup != null)
        {
            executeGroup.Visibility = Visibility.Collapsed;
            downloadGroup.Visibility = Visibility.Visible;
        }
    }

    private void DescargarVideojuego(object sender, RoutedEventArgs e)
    {
        VideogamesDetailViewModel.DescargarVideojuego();
    }

}

using CommunityToolkit.WinUI.UI.Animations;

using Excalinest.Contracts.Services;
using Excalinest.ViewModels;

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

        tagsList.ItemsSource = ViewModel.Item.Etiquetas;
    }

    private async void GetImageCover()
    {
        using (var memoryStream = new MemoryStream(ViewModel.Item.Portada.Data))
        {
            var bitmapImage = new BitmapImage();
            await bitmapImage.SetSourceAsync(memoryStream.AsRandomAccessStream());

            var imageControl = new Image();
            Cover.Source = bitmapImage;
        }
    }

    private async void GetImageTwitter()
    {
        using (var memoryStream = new MemoryStream(ViewModel.Item.Twitter.Data))
        {
            var bitmapImage = new BitmapImage();
            await bitmapImage.SetSourceAsync(memoryStream.AsRandomAccessStream());

            var imageControl = new Image();
            Twitter.Source = bitmapImage;
        }


    }

    private async void GetImageFacebook()
    {
        using (var memoryStream = new MemoryStream(ViewModel.Item.Facebook.Data))
        {
            var bitmapImage = new BitmapImage();
            await bitmapImage.SetSourceAsync(memoryStream.AsRandomAccessStream());

            var imageControl = new Image();
            Facebook.Source = bitmapImage;
        }
    }

    private async void GetImageInstagram()
    {
        using (var memoryStream = new MemoryStream(ViewModel.Item.Instagram.Data))
        {
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

    
}

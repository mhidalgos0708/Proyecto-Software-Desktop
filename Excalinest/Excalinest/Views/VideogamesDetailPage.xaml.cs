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

        if (ViewModel.Item != null)
        {
            tagsList.ItemsSource = ViewModel._listaEtiquetas;
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

    private async void DescargarVideojuego(object sender, RoutedEventArgs e)
    {
        await VideogamesDetailViewModel.DescargarVideojuego();
        var downloadGroup = FindName("downloadGroup") as StackPanel;
        var executeGroup = FindName("executeGroup") as StackPanel;

        if (downloadGroup != null && executeGroup != null)
        {
            downloadGroup.Visibility = Visibility.Collapsed;
            executeGroup.Visibility = Visibility.Visible;
            //layoutRoot.Visibility = Visibility.Visible;
        }
    }

}

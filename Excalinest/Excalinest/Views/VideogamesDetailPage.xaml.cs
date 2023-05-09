using System.Windows.Forms;
using CommunityToolkit.WinUI.UI.Animations;

using Excalinest.Contracts.Services;
using Excalinest.ViewModels;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Excalinest.Views;

public sealed partial class VideogamesDetailPage : Page
{
    public Microsoft.UI.Dispatching.DispatcherQueue TheDispatcher
    {
        get; set;
    }

    public VideogamesDetailViewModel ViewModel
    {
        get;
    }

    public VideogamesDetailPage()
    {
        ViewModel = App.GetService<VideogamesDetailViewModel>();
        InitializeComponent();
        TheDispatcher = this.DispatcherQueue;
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

    private async void EliminarVideojuego(object sender, RoutedEventArgs e)
    {
        var result = VideogamesDetailViewModel.EliminarVideojuego();
        string? message;

        if (result)
        {
            message = "Videojuego eliminado localmente con éxito.";
            var downloadGroup = FindName("downloadGroup") as StackPanel;
            var executeGroup = FindName("executeGroup") as StackPanel;

            if (downloadGroup != null && executeGroup != null)
            {
                executeGroup.Visibility = Visibility.Collapsed;
                downloadGroup.Visibility = Visibility.Visible;
            }
        }
        else
        {
            message = "Error al eliminar el videojuego localmente.";
        }

        ContentDialog dialog = new ContentDialog();
        dialog.XamlRoot = this.XamlRoot;
        dialog.Style = Microsoft.UI.Xaml.Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.Title = "Atención";
        dialog.PrimaryButtonText = "Ok";
        dialog.DefaultButton = ContentDialogButton.Primary;
        dialog.Content = new Dialog(message);
        await dialog.ShowAsync();
    }

    private async void DescargarVideojuego(object sender, RoutedEventArgs e)
    {
        var downloadGroup = FindName("downloadGroup") as StackPanel;
        var executeGroup = FindName("executeGroup") as StackPanel;
        var layoutRoot = FindName("layoutRoot") as StackPanel;

        if (downloadGroup != null && layoutRoot != null)
        {
            downloadGroup.Visibility = Visibility.Collapsed;
            layoutRoot.Visibility = Visibility.Visible;
        }

        System.Threading.Thread.Sleep(0000);

        try
        {
            _ = Task.Run(() => VideogamesDetailViewModel.DescargarVideojuego()).ContinueWith((t) =>
            {
                TheDispatcher.TryEnqueue(() =>
                {
                    if (executeGroup != null && layoutRoot != null)
                    {
                        executeGroup.Visibility = Visibility.Visible;
                        layoutRoot.Visibility = Visibility.Collapsed;
                    }
                });
            });
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

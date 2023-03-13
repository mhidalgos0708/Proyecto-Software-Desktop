using System.Diagnostics;
using CommunityToolkit.WinUI.UI.Animations;

using Excalinest.Contracts.Services;
using Excalinest.ViewModels;
using Excalinest.PatronesDiseño.ObserverTiempoInac;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Image = Microsoft.UI.Xaml.Controls.Image;

using System.Windows.Forms;
using WinUIEx;
using Windows.ApplicationModel.Core;
using Microsoft.UI.Xaml.Input;


namespace Excalinest.Views;

public sealed partial class VideogamesDetailPage : Page
{

    public VideogamesDetailViewModel ViewModel
    {
        get;
    }

    private PublisherTiempoInac NotificadorTiempoInac;
    private ISubscriberTiempoInac ObservadorTiempoInac;

    private string NombreVideojuego;

    private Process VideojuegoActual;

    private readonly int SegundosInactividad;

    public VideogamesDetailPage()
    {
        ViewModel = App.GetService<VideogamesDetailViewModel>();
        InitializeComponent();

        // Inicilizar atributos asociados a ejecutar
        SegundosInactividad = 60;
        VideojuegoActual = new Process();
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

    // Método para ejecutar el videojuego actual, busca en la ruta del mismo nombre el ejecutable correspondiente
    public void EjecutarVideojuego(object sender, RoutedEventArgs e)
    {
        NombreVideojuego = ViewModel.Item.Titulo;

        // Esta línea se utiliza debido a que las rutas relativas en c# se establecen desde system32
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory); // Establece a CurrentDirectory la ruta donde se buscan los archivos ensamblador y se encuentran los videojuegos ejecutables

        var RutaJuego =  @".\Assets\Videojuegos\" + NombreVideojuego;
        
        try
        {
            // Obtener los nombres de archvios ejecutables dentro de la carpeta del juego actual
            var VideojuegoEjecutable = Directory.GetFiles(RutaJuego, "*.exe", SearchOption.AllDirectories) // Retorna una lista de archivos .exe dentro de la carpeta RutaJuego
                    .Where(archivo => !archivo.Contains("UnityCrashHandler"))
                    .AsEnumerable()
                    .ToArray();

            VideojuegoActual.StartInfo.UseShellExecute = false; // Ejecutar directamente desde el archivo ejecutable
            VideojuegoActual.StartInfo.FileName = VideojuegoEjecutable[0]; // Establecer la ruta del archivo ejecutable
            VideojuegoActual.StartInfo.CreateNoWindow = true; // Abrir una nueva ventana
            VideojuegoActual.Start();

            NotificadorTiempoInac = new PublisherTiempoInac(SegundosInactividad*1000);
            ObservadorTiempoInac = new SubscriberTiempoInac(VideojuegoActual);
            NotificadorTiempoInac.Suscribirse(ObservadorTiempoInac);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    ~VideogamesDetailPage()
    {
        VideojuegoActual.Dispose(); // Borrar memoria del proceso una vez se accede a otra página
    }
}

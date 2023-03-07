using CommunityToolkit.WinUI.UI.Animations;

using Excalinest.Contracts.Services;
using Excalinest.Core.Models;
using Excalinest.ViewModels;
using Excalinest.PatronesDiseño.ObserverTiempoInac;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Excalinest.Views;

// Código para la ventana de detalles de cada videojuego
public sealed partial class VideogamesDetailPage : Page
{
    public VideogamesDetailViewModel ViewModel
    {
        get;
    }

    private PublisherTiempoInac NotificadorTiempoInac;
    private ISubscriberTiempoInac ObservadorTiempoInac;

    private readonly string NombreVideojuego;

    private Process VideojuegoActual;

    public VideogamesDetailPage()
    {
        ViewModel = App.GetService<VideogamesDetailViewModel>();
        InitializeComponent();
        NombreVideojuego = "Wednesday";
        VideojuegoActual = new Process();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        this.RegisterElementForConnectedAnimation("animationKeyContentGrid", itemHero);
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
    // Se hace la suposición de que el ejecutable se llama igual que el juego **Importante preguntar**
    // Cambiar posible pulga en línea var VideojuegoActual = Process.Start(VideojuegoEjecutable[i]); por ejecución de directa del crashhandler

    public void EjecutarVideojuego(object sender, RoutedEventArgs e)
    {
        // Esta línea se utiliza debido a que las rutas relativas en c# se establecen desde system32
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory); // Establece a CurrentDirectory la ruta donde se buscan los archivos ensamblador y se encuentran los videojuegos ejecutables

        var RutaJuego =  @".\Assets\Videojuegos\" + NombreVideojuego;
        
        try
        {
            // Obtener los nombres de archvios ejecutables dentro de la carpeta del juego actual
            var VideojuegoEjecutable = Directory.GetFiles(RutaJuego, "*.exe", SearchOption.AllDirectories) // Retorna una lista de archivos .exe dentro de la carpeta RutaJuego
                    .AsEnumerable()
                    .ToArray();

            //for (var i = 0; i < VideojuegoEjecutable.Length; i++)
            //{
                // Ejecutar el archivo .exe del videojuego actual
                VideojuegoActual.StartInfo.UseShellExecute = false; // Ejecutar directamente desde el archivo ejecutable
                VideojuegoActual.StartInfo.FileName = VideojuegoEjecutable[1]; // Establecer la ruta del archivo ejecutable
                VideojuegoActual.StartInfo.CreateNoWindow = true; // Abrir una nueva ventana
                VideojuegoActual.Start(); 

                NotificadorTiempoInac = new PublisherTiempoInac(20000);
                ObservadorTiempoInac = new SubscriberTiempoInac(VideojuegoActual);
                NotificadorTiempoInac.Suscribirse(ObservadorTiempoInac);
            //}
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

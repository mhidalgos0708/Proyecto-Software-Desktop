using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;

using Excalinest.Contracts.ViewModels;
using Excalinest.Core.Contracts.Services;
using Excalinest.Core.Models;
using Excalinest.Core.Services;
using Excalinest.PatronesDiseño.ObserverTiempoInac;

namespace Excalinest.ViewModels;

public class VideogamesDetailViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;
    private static ServicioVideojuego? servicioVideojuego;
    private Videojuego? _item;

    private static PublisherTiempoInac? NotificadorTiempoInac;
    private static ISubscriberTiempoInac? ObservadorTiempoInac;

    private static string NombreVideojuego = "";

    private static int SegundosInactividad = 0;
    private static string RutaJuego = "";

    public Videojuego? Item
    {
        get => _item;
        set => SetProperty(ref _item, value);
    }

    public VideogamesDetailViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
        servicioVideojuego = new ServicioVideojuego(new MongoConnection());
        SegundosInactividad = 60;
    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is string titulo)
        {
            if (servicioVideojuego != null)
            {
                Item = await servicioVideojuego.GetVideojuegoPorTitulo(titulo);
                NombreVideojuego = Item.Titulo;
            }
            RutaJuego = @"..\..\VideojuegosExcalinest\";
        }
    }

    public void OnNavigatedFrom()
    {
    }

    // Método para ejecutar el videojuego actual, busca en la ruta del mismo nombre el ejecutable correspondiente
    public static void EjecutarVideojuego()
    {

        // Esta línea se utiliza debido a que las rutas relativas en c# se establecen desde system32
        // Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory); // Establece a CurrentDirectory la ruta donde se buscan los archivos ensamblador y se encuentran los videojuegos ejecutables

        try
        {
            // Obtener los nombres de archivos ejecutables dentro de la carpeta del juego actual
            var VideojuegoEjecutable = Directory.GetFiles(RutaJuego + NombreVideojuego, "*.exe", SearchOption.AllDirectories) // Retorna una lista de archivos .exe dentro de la carpeta RutaJuego
                    .Where(archivo => !archivo.Contains("UnityCrashHandler"))
                    .AsEnumerable()
                    .ToArray();

            NotificadorTiempoInac = new PublisherTiempoInac(SegundosInactividad * 1000);
            ObservadorTiempoInac = new SubscriberTiempoInac(VideojuegoEjecutable[0]);
            NotificadorTiempoInac.Suscribirse(ObservadorTiempoInac);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public static void EliminarVideojuego()
    {
        try
        {
            if (Directory.Exists(RutaJuego + NombreVideojuego))
            {
                var files = Directory.GetFiles(RutaJuego + NombreVideojuego);
                var subdirectories = Directory.GetDirectories(RutaJuego + NombreVideojuego);

                foreach (var file in files)
                {
                    File.Delete(file);
                    while (File.Exists(file))
                    {
                        Thread.Sleep(100);
                    }
                }

                foreach (var subdirectory in subdirectories)
                {
                    Directory.Delete(subdirectory, true);
                    while (Directory.Exists(subdirectory))
                    {
                        Thread.Sleep(100);
                    }
                }

                Directory.Delete(RutaJuego + NombreVideojuego);

                while (Directory.Exists(RutaJuego + NombreVideojuego))
                {
                    Thread.Sleep(100);
                }

                MessageBox.Show("Videojuego eliminado localmente con éxito.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex) 
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public static bool EsVideojuegoDescargado()
    {
        return Directory.Exists(RutaJuego + NombreVideojuego);
    }

    public static async Task<bool> DescargarVideojuego()
    {
        await Task.CompletedTask;
        if (servicioVideojuego != null)
        {
            var res = await servicioVideojuego.DownloadVideojuego(RutaJuego, NombreVideojuego + ".zip");
            MessageBox.Show(res, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }
        return true;
    } 
}

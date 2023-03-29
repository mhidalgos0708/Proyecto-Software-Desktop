using System.Collections.ObjectModel;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;

using Excalinest.Contracts.ViewModels;
using Excalinest.Core.Contracts.Services;
using Excalinest.Core.Models;
using Excalinest.Core.Services;
using Excalinest.PatronesDiseño.ObserverTiempoInac;
using Microsoft.UI.Xaml;
using System.IO;

using System.Diagnostics;

namespace Excalinest.ViewModels;

public class VideogamesDetailViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;
    private readonly ServicioVideojuego servicioVideojuego;
    private Videojuego? _item;

    private static PublisherTiempoInac? NotificadorTiempoInac;
    private static ISubscriberTiempoInac? ObservadorTiempoInac;

    private static string NombreVideojuego = "";

    private static readonly int SegundosInactividad = 60;
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
    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is string titulo)
        {
            Item = await servicioVideojuego.GetVideojuegoPorTitulo(titulo);
            NombreVideojuego = Item.Titulo;
            RutaJuego = @"..\..\VideojuegosExcalinest\" + NombreVideojuego;
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
            // Obtener los nombres de archvios ejecutables dentro de la carpeta del juego actual
            var VideojuegoEjecutable = Directory.GetFiles(RutaJuego, "*.exe", SearchOption.AllDirectories) // Retorna una lista de archivos .exe dentro de la carpeta RutaJuego
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
            if (Directory.Exists(RutaJuego))
            {
                var files = Directory.GetFiles(RutaJuego);
                var subdirectories = Directory.GetDirectories(RutaJuego);

                foreach (var file in files)
                {
                    File.Delete(file);
                }

                foreach (var subdirectory in subdirectories)
                {
                    Directory.Delete(subdirectory, true);
                }

                Directory.Delete(RutaJuego);

                MessageBox.Show("Videojuego eliminado de la unidad C:/ exitosamente.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (Exception ex) 
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public static bool EsVideojuegoDescargado()
    {
        return Directory.Exists(RutaJuego);
    }

    public static void DescargarVideojuego()
    {
        MessageBox.Show("Descargando "+NombreVideojuego+"...", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);
    } 
}

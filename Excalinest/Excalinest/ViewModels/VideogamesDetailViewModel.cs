using System.Collections.ObjectModel;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;

using Excalinest.Contracts.ViewModels;
using Excalinest.Core.Contracts.Services;
using Excalinest.Core.Models;
using Excalinest.Core.Services;
using Excalinest.PatronesDiseño.ObserverTiempoInac;
using Microsoft.UI.Xaml;

using System.Diagnostics;

namespace Excalinest.ViewModels;

public class VideogamesDetailViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;
    private readonly ServicioVideojuego servicioVideojuego;
    private Videojuego? _item;

    private PublisherTiempoInac NotificadorTiempoInac;
    private ISubscriberTiempoInac ObservadorTiempoInac;

    private string NombreVideojuego;

    private readonly int SegundosInactividad;

    public Videojuego? Item
    {
        get => _item;
        set => SetProperty(ref _item, value);
    }

    public VideogamesDetailViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
        servicioVideojuego = new ServicioVideojuego(new MongoConnection());

        // Inicilizar atributos asociados a ejecutar
        SegundosInactividad = 60;
    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is string titulo)
        {
            Item = await servicioVideojuego.GetVideojuegoPorTitulo(titulo);
        }
    }

    public void OnNavigatedFrom()
    {
    }

    // Método para ejecutar el videojuego actual, busca en la ruta del mismo nombre el ejecutable correspondiente
    public void EjecutarVideojuego(object sender, RoutedEventArgs e)
    {
        NombreVideojuego = Item.Titulo;

        // Esta línea se utiliza debido a que las rutas relativas en c# se establecen desde system32
        // Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory); // Establece a CurrentDirectory la ruta donde se buscan los archivos ensamblador y se encuentran los videojuegos ejecutables

        var RutaJuego = @"C:\Excalinest\VideojuegosExcalinest\" + NombreVideojuego;

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
}

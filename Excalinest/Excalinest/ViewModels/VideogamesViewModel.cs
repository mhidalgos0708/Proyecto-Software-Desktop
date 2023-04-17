using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.UI;
using CommunityToolkit.WinUI.UI.Controls;
using Excalinest.Contracts.Services;
using Excalinest.Contracts.ViewModels;
using Excalinest.Core.Contracts.Services;
using Excalinest.Core.Models;
using Excalinest.Core.Services;
using Excalinest.Views;
using Microsoft.IdentityModel.Tokens;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualBasic.Logging;
using Microsoft.Graph;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Directory = System.IO.Directory;

namespace Excalinest.ViewModels;

public class VideogamesViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly ISampleDataService _sampleDataService;
    public ServicioVideojuego _videojuegoService;
    public ServicioEtiqueta _etiquetaService;
    public static ServicioVideojuego _videojuegoService;

    private static readonly string RutaJuego = @"..\..\VideojuegosExcalinest\";

    private static List<Videojuego> _videojuegosSeleccionados;
    public ICommand ItemClickCommand
    {
        get;
    }

    public ObservableCollection<Videojuego> Source { get; } = new ObservableCollection<Videojuego>();
    public ObservableCollection<Tag> Tags { get; } = new ObservableCollection<Tag>();


    public VideogamesViewModel(INavigationService navigationService, ISampleDataService sampleDataService)
    {
        _navigationService = navigationService;
        _sampleDataService = sampleDataService;
        _videojuegoService = new ServicioVideojuego(new MongoConnection());
        _etiquetaService = new ServicioEtiqueta(new MongoConnection());
        _videojuegosSeleccionados = new List<Videojuego>();

        ItemClickCommand = new RelayCommand<Videojuego>(OnItemClick);
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        Tag defaultValue = new Tag();
        defaultValue.ID = -1;
        defaultValue.Nombre = "Todos";

        Tags.Add(defaultValue);

        var tags = await _etiquetaService.GetTags();
        foreach (var item in tags)
        {
            Tags.Add(item);
        }

        
        // TODO: Replace with real data.
        var data = await _videojuegoService.GetVideojuegos();
        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    } 

    private void OnItemClick(Videojuego? clickedItem)
    {
        if (clickedItem != null)
        {
            _navigationService.SetListDataItemForNextConnectedAnimation(clickedItem);
            _navigationService.NavigateTo(typeof(VideogamesDetailViewModel).FullName!, clickedItem.Titulo);
        }
    }

    public async Task GetVideojuegosByTag(int ID)
    {
        if(ID == -1)
        {
            Source.Clear();

            var data = await _videojuegoService.GetVideojuegos();
            foreach (var item in data)
            {
                Source.Add(item);
            }
        }
        else
        {
            Source.Clear();

            var data = await _videojuegoService.GetVideojuegosByTagID(ID);
            foreach (var item in data)
            {
                Source.Add(item);
            }
        }
    }


    // Métodos de administración de lista de videojuegos seleccionados
    public static void AgregarJuegoListaDescarga(Videojuego videojuego)
    {
        _videojuegosSeleccionados.Add(videojuego);
    }

    public static void QuitarJuegoListaDescarga(Videojuego videojuego)
    {
        _videojuegosSeleccionados.Remove(videojuego);
    }

    public static int CantidadVideojuegosSeleccionados()
    {
        return _videojuegosSeleccionados.Count;
    }

    public static List<Videojuego> ObtenerVideojuegosSeleccionados()
    {
        return _videojuegosSeleccionados;
    }

    public static void LimpiarVideojuegosSeleccionados()
    {
        _videojuegosSeleccionados.Clear();
    }

    // Métodos para buscar videjuegos descargados
    public static List<bool> BuscarVideojuegos(ItemCollection videojuegos)
    {
        var estanDescargados = new List<bool>();    

        foreach(var videojuego in videojuegos.Cast<Videojuego>()) 
        {
            if(Directory.Exists(RutaJuego + videojuego.Titulo))
            {
                estanDescargados.Add(true);
            }
            else
            {
                estanDescargados.Add(false);
            }
        }

        return estanDescargados;
    }

    public static bool BuscarVideojuego(Videojuego videojuego)
    {
        return Directory.Exists(RutaJuego + videojuego.Titulo);
    }

    public static async Task<bool> DescargarVideojuegos()
    {
        await Task.CompletedTask;
        if (_videojuegoService != null)
        {
            foreach (var videojuego in _videojuegosSeleccionados)
            {
                var res = await _videojuegoService.DownloadVideojuego(RutaJuego, videojuego.Titulo + ".zip");
                MessageBox.Show(res, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return true;
        }
        return true;
    }
}

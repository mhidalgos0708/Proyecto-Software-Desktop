using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Excalinest.Contracts.Services;
using Excalinest.Contracts.ViewModels;
using Excalinest.Core.Contracts.Services;
using Excalinest.Core.Models;
using Excalinest.Core.Services;
using Excalinest.Services;
using Excalinest.Strings;
using Excalinest.Views;
using Microsoft.IdentityModel.Tokens;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualBasic.Logging;
using Windows.Networking.Connectivity;

namespace Excalinest.ViewModels;

public class MainViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly ISampleDataService _sampleDataService;
    public ServicioVideojuego _videojuegoService;
    public ServicioEtiqueta _etiquetaService;
    public ServicioVideojuegoEtiqueta _videojuegoEtiquetaService;

    ManejoArchivos _manejoArchivos = new ManejoArchivos();

    public ICommand ItemClickCommand
    {
        get;
    }

    public ObservableCollection<Videojuego> Source { get; } = new ObservableCollection<Videojuego>();
    public ObservableCollection<Tag> Tags { get; } = new ObservableCollection<Tag>();


    public String path = "";

    GlobalFunctions globalFunctions;

    public MainViewModel(INavigationService navigationService, ISampleDataService sampleDataService)
    {
        _navigationService = navigationService;
        _sampleDataService = sampleDataService;
        _videojuegoService = new ServicioVideojuego(new MongoConnection());
        _etiquetaService = new ServicioEtiqueta(new MongoConnection());
        _videojuegoEtiquetaService = new ServicioVideojuegoEtiqueta(new MongoConnection());

        ItemClickCommand = new RelayCommand<Videojuego>(OnItemClick);
        
    }

    public async void OnNavigatedTo(object parameter)
    {
        globalFunctions = new GlobalFunctions();
        
        Source.Clear();

        Tag defaultValue = new Tag();
        defaultValue.ID = -1;
        defaultValue.Nombre = "Todos";

        Tags.Add(defaultValue);

        try
        {
            var tags = await _etiquetaService.GetTags();
            foreach (var item in tags)
            {
                Tags.Add(item);
            }

            path = _manejoArchivos.leerRutaArchivos();

            // TODO: Replace with real data.
            var data = await _videojuegoService.GetVideojuegos();
            foreach (var item in data)
            {
                if (Directory.Exists(path + item.Titulo))
                {
                    Source.Add(item);
                }
            }
        }
        catch (Exception ex){
            if (!globalFunctions.CheckInternetConnectivity())
            {
                 MessageBox.Show("No hay conexión a Internet", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
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
        path =  _manejoArchivos.leerRutaArchivos(); ;
        if (ID == -1)
        {
            Source.Clear();

            var data = await _videojuegoService.GetVideojuegos();
            foreach (var item in data)
            {
                if (Directory.Exists(path+item.Titulo))
                {
                    Source.Add(item);
                }
            }
        }
        else
        {
            Source.Clear();

            var data = await _videojuegoEtiquetaService.GetVideojuegosByTagId(ID);
            foreach (var item in data)
            {
                if (Directory.Exists(path+item.Titulo))
                {
                    Source.Add(item);
                }
            }
        }
    }

}

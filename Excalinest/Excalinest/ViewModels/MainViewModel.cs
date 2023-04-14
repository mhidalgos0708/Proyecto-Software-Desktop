using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Excalinest.Contracts.Services;
using Excalinest.Contracts.ViewModels;
using Excalinest.Core.Contracts.Services;
using Excalinest.Core.Models;
using Excalinest.Core.Services;
using Excalinest.Views;
using Microsoft.IdentityModel.Tokens;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualBasic.Logging;

namespace Excalinest.ViewModels;

public class MainViewModel : ObservableRecipient, INavigationAware
{
    private readonly INavigationService _navigationService;
    private readonly ISampleDataService _sampleDataService;
    public ServicioVideojuego _videojuegoService;

    public ICommand ItemClickCommand
    {
        get;
    }

    public ObservableCollection<Videojuego> Source { get; } = new ObservableCollection<Videojuego>();
    public ObservableCollection<Tag> Tags { get; } = new ObservableCollection<Tag>();


    public String path = "";


    public MainViewModel(INavigationService navigationService, ISampleDataService sampleDataService)
    {
        _navigationService = navigationService;
        _sampleDataService = sampleDataService;
        _videojuegoService = new ServicioVideojuego(new MongoConnection());

        ItemClickCommand = new RelayCommand<Videojuego>(OnItemClick);
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        Tag defaultValue = new Tag();
        defaultValue.ID = -1;
        defaultValue.Nombre = "Todos";

        Tags.Add(defaultValue);

        var tags = await _videojuegoService.GetTags();
        foreach (var item in tags)
        {
            Tags.Add(item);
        }

        path = File.ReadLines(@"C:/Excalinest/VideojuegosExcalinest/config.txt").Last();
        
        // TODO: Replace with real data.
        var data = await _videojuegoService.GetVideojuegos();
        foreach (var item in data)
        {
            if (Directory.Exists(path+item.Titulo))
            {
                Source.Add(item);
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
        path = File.ReadLines(@"C:/Excalinest/VideojuegosExcalinest/config.txt").Last();
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

            var data = await _videojuegoService.GetVideojuegosByTagID(ID);
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

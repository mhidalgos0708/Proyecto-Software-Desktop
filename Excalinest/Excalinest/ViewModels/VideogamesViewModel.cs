using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Excalinest.Contracts.Services;
using Excalinest.Contracts.ViewModels;
using Excalinest.Core.Contracts.Services;
using Excalinest.Core.Models;
using Excalinest.Core.Services;
using Microsoft.VisualBasic.Logging;

namespace Excalinest.ViewModels;

public class VideogamesViewModel : ObservableRecipient, INavigationAware
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

    public VideogamesViewModel(INavigationService navigationService, ISampleDataService sampleDataService)
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

        
        // TODO: Replace with real data.
        var data = await _videojuegoService.GetVideojuegos();
        foreach (var item in data)
        {
            if (File.Exists(@"C:\Excalinest\VideojuegosExcalinest\"+item.Titulo+".exe"))
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
        if(ID == -1)
        {
            Source.Clear();

            var data = await _videojuegoService.GetVideojuegos();
            foreach (var item in data)
            {
                if (File.Exists(@"C:\Excalinest\VideojuegosExcalinest\"+item.Titulo+".exe"))
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
                if (File.Exists(@"C:\Excalinest\VideojuegosExcalinest\"+item.Titulo+".exe"))
                {
                    Source.Add(item);
                }
            }
        }
    }
}

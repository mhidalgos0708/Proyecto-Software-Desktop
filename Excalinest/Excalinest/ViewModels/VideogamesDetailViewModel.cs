using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

using Excalinest.Contracts.ViewModels;
using Excalinest.Core.Contracts.Services;
using Excalinest.Core.Models;
using Excalinest.Core.Services;

namespace Excalinest.ViewModels;

public class VideogamesDetailViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;
    private readonly ServicioVideojuego servicioVideojuego;
    private Videojuego? _item;

    public ObservableCollection<string> Tags { get; } = new ObservableCollection<string>();

    public Videojuego? Item
    {
        get => _item;
        set => SetProperty(ref _item, value);
    }

    public VideogamesDetailViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
        servicioVideojuego = new ServicioVideojuego();
    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is string titulo)
        {
            var data = await _sampleDataService.GetContentGridDataAsync();
            Item = await servicioVideojuego.GetVideojuegoPorTitulo(titulo);
            Tags.Add("2D");
        }
    }

    public void OnNavigatedFrom()
    {
    }
}

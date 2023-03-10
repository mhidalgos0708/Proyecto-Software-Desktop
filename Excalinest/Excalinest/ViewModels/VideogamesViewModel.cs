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
        _videojuegoService = new ServicioVideojuego();

        ItemClickCommand = new RelayCommand<Videojuego>(OnItemClick);
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        var tags = await _videojuegoService.GetTags();
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
}

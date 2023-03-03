using CommunityToolkit.WinUI.UI.Animations;

using Excalinest.Contracts.Services;
using Excalinest.Core.Models;
using Excalinest.ViewModels;

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

    private readonly string NombreVideojuego;

    public VideogamesDetailPage()
    {
        ViewModel = App.GetService<VideogamesDetailViewModel>();
        InitializeComponent();
        NombreVideojuego = "Wednesday";
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
    public void EjecutarVideojuego(object sender, RoutedEventArgs e)
    {
        Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

        var RutaJuego =  @".\Assets\Videojuegos\" + NombreVideojuego;
        
        try
        {
            var VideojuegoEjecutable = Directory.GetFiles(RutaJuego, "*.exe", SearchOption.AllDirectories)
                    .AsEnumerable()
                    .ToArray();

            for (var i = 0; i < VideojuegoEjecutable.Length; i++)
            {
                var VideojuegoActual = Process.Start(VideojuegoEjecutable[i]);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

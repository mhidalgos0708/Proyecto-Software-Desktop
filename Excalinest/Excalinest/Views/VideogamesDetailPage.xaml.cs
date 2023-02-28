using CommunityToolkit.WinUI.UI.Animations;

using Excalinest.Contracts.Services;
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
    public void EjecutarVideojuego(object sender, RoutedEventArgs e)
    {
        var NombreExtension = NombreVideojuego + ".EXE";
        var RutaJuego = @"Excalinest\Assets\Videojuegos\" + NombreVideojuego;
        try
        {
            var VideojuegoEjecutable = Directory.GetFiles("D:\\", NombreExtension, SearchOption.AllDirectories)
                    .Select(fileName => Path.GetFileNameWithoutExtension(fileName))
                    .AsEnumerable()
                    .ToArray();

            for (var i = 0; i < VideojuegoEjecutable.Length; i++)
            {
                if (Path.GetFileName(Path.GetFileNameWithoutExtension(VideojuegoEjecutable[i])) == NombreVideojuego)
                {
                    var VideojuegoActual = Process.Start(VideojuegoEjecutable[i]);
                }
            }
            
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

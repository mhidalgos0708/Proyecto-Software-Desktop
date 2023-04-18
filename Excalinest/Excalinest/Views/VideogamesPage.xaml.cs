using System.Diagnostics;
using Excalinest.Core.Models;
using Excalinest.ViewModels;
using Excalinest.Services;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Media;
using MessageBox = System.Windows.Forms.MessageBox;
using MessageBoxButtons = System.Windows.Forms.MessageBoxButtons;
using MessageBoxIcon = System.Windows.Forms.MessageBoxIcon;
using CommunityToolkit.WinUI.UI.Controls;
using Windows.Foundation.Collections;
using static System.Net.Mime.MediaTypeNames;
using CommunityToolkit.WinUI.UI;

using Microsoft.UI.Xaml.Media;
using Microsoft.UI;

namespace Excalinest.Views;

public sealed partial class VideogamesPage : Page
{

    public VideogamesViewModel ViewModel
    {
        get;
    }

    public VideogamesPage()
    {
        ViewModel = App.GetService<VideogamesViewModel>();
        InitializeComponent();

        /*if (GridView.DataFetchSize ==0)
        {
            gamesOrNot.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
        }*/
        // taglist.ItemsSource = ViewModel.Tags;
    }

    public async void TagComboBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        ComboBox tag = (ComboBox)sender;
        Tag chosenTag = tag.SelectedItem as Tag;
        
        await ViewModel.GetVideojuegosByTag(chosenTag.ID);
    }

    // Agregar videojuego a lista de videojuegos por descargar al activar su respectiva casilla de selección múltiple
    public void CheckBoxSeleccion_Checked(object sender, RoutedEventArgs e)
    {
        var checkbox = (CheckBox)sender;
        var videojuegoActual = (Videojuego)checkbox.Tag;

        VideogamesViewModel.AgregarJuegoListaDescarga(videojuegoActual);

        //Mostrar el botón de descargar al seleccionar el primer videojuego
        var botonDescargar = FindName("BotonDescargar") as Button;
        var mostrarBoton = VideogamesViewModel.CantidadVideojuegosSeleccionados();

        if (botonDescargar != null && mostrarBoton == 1) {
            botonDescargar.Visibility = Visibility.Visible;
        }
    }

    // Quitar videojuego de lista de videojuegos por descargar al desactivar su respectiva casilla de selección múltiple
    public void CheckBoxSeleccion_UnChecked(object sender, RoutedEventArgs e)
    {
        var checkbox = (CheckBox)sender;
        var videojuegoActual = (Videojuego)checkbox.Tag;

        VideogamesViewModel.QuitarJuegoListaDescarga(videojuegoActual);

        //Ocultar el botón de descargar una vez no hay videojuegos selccionados
        var botonDescargar = FindName("BotonDescargar") as Button;
        var mostrarBoton = VideogamesViewModel.CantidadVideojuegosSeleccionados();

        if (botonDescargar != null && mostrarBoton == 0)
        {
            botonDescargar.Visibility = Visibility.Collapsed;
        }
    }

    //Método para buscar elementos visuales de un contenedor generado por AdaptativeGridView
    private T? BuscarItemsVisuales<T>(DependencyObject elementoVisualPadre, string nombreElementoVisualHijo) where T : FrameworkElement
    {
        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(elementoVisualPadre); i++) 
        {
            var elementoVisualHijo = VisualTreeHelper.GetChild(elementoVisualPadre, i); // Obtener el elemento visual hijo en la posición i
            if (elementoVisualHijo is T hijoActual && hijoActual.Name == nombreElementoVisualHijo)
            {
                return hijoActual;
            }

            // Si el hijo actual no coincide con el nombre deseado, busca recursivamente en los hijos del elemento hijo actual
            var result = BuscarItemsVisuales<T>(elementoVisualHijo, nombreElementoVisualHijo);
            if (result != null)
            {
                return result;
            }
        }

        // Si no se encuentra el elemento visual deseado, se devuelve null
        return null;
    }

    public async void DescargarVideojuegos(object sender, RoutedEventArgs e)
    {
        await VideogamesViewModel.DescargarVideojuegos();

        // Desactivar checkboxes de juegos descargados
        var gridVideojuegos = FindName("GridVideojuegos") as AdaptiveGridView;

        if (gridVideojuegos != null)
        {
            var videojuegosSeleccionados = VideogamesViewModel.ObtenerVideojuegosSeleccionados();

            foreach (var videojuego in videojuegosSeleccionados)
            {
                var contenedorVideojuego = gridVideojuegos.ContainerFromItem(videojuego);

                if (contenedorVideojuego != null)
                {
                    var zonaCheckBoxSeleccion = BuscarItemsVisuales<StackPanel>(contenedorVideojuego, "ZonaCheckBoxSeleccion");
                    if (zonaCheckBoxSeleccion != null)
                    {
                        zonaCheckBoxSeleccion.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        VideogamesViewModel.LimpiarVideojuegosSeleccionados();
    }

    private void CajaVideojuego_PointerEntered(object sender, PointerRoutedEventArgs e)
    {
        var cajaVideojuego = (Border)sender;

        cajaVideojuego.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 230, 230, 230));

        // Mostrar CheckBox o signo de Check dependiendo de si esta descargado o no
        var zonaCheckBoxSeleccion = cajaVideojuego.FindName("ZonaCheckBoxSeleccion") as StackPanel;

        if (zonaCheckBoxSeleccion != null)
        {
            var estaDescargado = VideogamesViewModel.BuscarVideojuego((Videojuego)zonaCheckBoxSeleccion.Tag);

            if (estaDescargado)
            {
                var descargado = cajaVideojuego.FindName("Descargado") as SymbolIcon;
                if (descargado != null)
                {
                    descargado.Visibility = Visibility.Visible;
                }
            }
            else
            {
                zonaCheckBoxSeleccion.Visibility = Visibility.Visible;
            }
        }
    }

    private void CajaVideojuego_PointerExited(object sender, PointerRoutedEventArgs e)
    {
        var cajaVideojuego = (Border)sender;

        cajaVideojuego.Background = new SolidColorBrush(Colors.Transparent);

        // Esconder CheckBox o signo de Check dependiendo de si esta descargado o no
        var zonaCheckBoxSeleccion = cajaVideojuego.FindName("ZonaCheckBoxSeleccion") as StackPanel;

        var videojuegosSeleccionados = VideogamesViewModel.ObtenerVideojuegosSeleccionados();

        if (zonaCheckBoxSeleccion != null)
        {
            var videojuego = (Videojuego) zonaCheckBoxSeleccion.Tag;
            var estaDescargado = VideogamesViewModel.BuscarVideojuego(videojuego);

            if (!videojuegosSeleccionados.Contains(videojuego))
            {
                if (estaDescargado)
                {
                    var descargado = cajaVideojuego.FindName("Descargado") as SymbolIcon;
                    if (descargado != null)
                    {
                        descargado.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    zonaCheckBoxSeleccion.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
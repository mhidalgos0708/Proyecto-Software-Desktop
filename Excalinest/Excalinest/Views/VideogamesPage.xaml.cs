﻿using Excalinest.Core.Models;
using Excalinest.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI;
using CommunityToolkit.WinUI.UI.Animations;
using Microsoft.UI.Xaml.Navigation;
using Excalinest.Strings;

namespace Excalinest.Views;

public sealed partial class VideogamesPage : Page
{

    private GlobalFunctions _globalFunctions;

    public Microsoft.UI.Dispatching.DispatcherQueue TheDispatcher
    {
        get; set;
    }
    public VideogamesViewModel ViewModel
    {
        get;
    }

    public VideogamesPage()
    {
        ViewModel = App.GetService<VideogamesViewModel>();
        
        InitializeComponent();
        TheDispatcher = this.DispatcherQueue;

        _globalFunctions = new GlobalFunctions();

        if (_globalFunctions.CheckInternetConnectivity())
        {
            var infoBar = FindName("infoBar") as StackPanel;

            if (infoBar != null)
            {
                infoBar.Visibility = Visibility.Collapsed;
            }
        }
    }
    
    public void DesactivarCheckboxes(List<Videojuego> videojuegos)
    {
        var gridVideojuegos = FindName("GridVideojuegos") as AdaptiveGridView;

        var checkBoxesList = new List<CheckBox>();

        if (gridVideojuegos != null)
        {
            foreach (var videojuego in videojuegos)
            {
                var contenedorVideojuego = gridVideojuegos.ContainerFromItem(videojuego);

                if (contenedorVideojuego != null)
                {
                    var zonaCheckBoxSeleccion = BuscarItemsVisuales<StackPanel>(contenedorVideojuego, "ZonaCheckBoxSeleccion");
                    if (zonaCheckBoxSeleccion != null)
                    {
                        zonaCheckBoxSeleccion.Visibility = Visibility.Collapsed;

                        var checkBoxSeleccion = BuscarItemsVisuales<CheckBox>(contenedorVideojuego, "CheckBoxSeleccion");

                        if(checkBoxSeleccion != null)
                        {
                            checkBoxesList.Add(checkBoxSeleccion);
                        }
                    }
                }
            }

            foreach (var checkbox in checkBoxesList)
            {
                checkbox.IsChecked = false;
            }
        }

        VideogamesViewModel.LimpiarVideojuegosSeleccionados();
    }

    public void DesactivarCheckbox(Videojuego videojuego)
    {
        var gridVideojuegos = FindName("GridVideojuegos") as AdaptiveGridView;

        if (gridVideojuegos != null)
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

    public async void TagComboBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        if (_globalFunctions.CheckInternetConnectivity())
        {
            ComboBox tag = (ComboBox)sender;
            Tag chosenTag = (Tag)tag.SelectedItem;

            try
            {
                // Limpiar selección de videojuegos al filtrar por etiqueta

                var botonDescargar = FindName("BotonDescargar") as Button;
                if (botonDescargar != null)
                {
                    botonDescargar.Visibility = Visibility.Collapsed;
                }

                var videojuegosSeleccionados = VideogamesViewModel.ObtenerVideojuegosSeleccionados();

                DesactivarCheckboxes(videojuegosSeleccionados);

                await ViewModel.GetVideojuegosByTag(chosenTag.ID);
            }
            catch (Exception ex)
            {
                ContentDialog dialog = new ContentDialog();
                dialog.XamlRoot = this.XamlRoot;
                dialog.Style = Microsoft.UI.Xaml.Application.Current.Resources["DefaultContentDialogStyle"] as Style;
                dialog.Title = "Atención";
                dialog.PrimaryButtonText = "Ok";
                dialog.DefaultButton = ContentDialogButton.Primary;

                var message = "Error: " + ex;
                dialog.Content = new Dialog(message);
                await dialog.ShowAsync();
            }
        }
        
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
        var botonDescargar = FindName("BotonDescargar") as Button;
        var progressBar = FindName("layoutRoot") as StackPanel;

        if (botonDescargar != null && progressBar != null)
        {
            botonDescargar.Visibility = Visibility.Collapsed;
            progressBar.Visibility = Visibility.Visible;
        }

        ContentDialog dialog = new ContentDialog();
        dialog.XamlRoot = this.XamlRoot;
        dialog.Style = Microsoft.UI.Xaml.Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.Title = "Atención";
        dialog.PrimaryButtonText = "Ok";
        dialog.DefaultButton = ContentDialogButton.Primary;

        var message = "";

        try
        {
            var videojuegosSeleccionados = VideogamesViewModel.ObtenerVideojuegosSeleccionados();

            foreach(var videojuego in videojuegosSeleccionados)
            {
                var progressBarText = FindName("ProgressBarText") as TextBlock;
                if(progressBarText != null)
                {
                    progressBarText.Text = "Descargando " + videojuego.Titulo + "...";
                }

                await Task.Run(() => message = DescargarVideoJuegoSegundoPlano(videojuego.Titulo).Result).ContinueWith((t) =>
                {
                    TheDispatcher.TryEnqueue(async () =>
                    {
                        if(videojuego.Titulo == videojuegosSeleccionados.Last().Titulo)
                        {
                            progressBarText.Visibility = Visibility.Collapsed;
                        }

                        DesactivarCheckbox(videojuego);
                        dialog.Content = new Dialog(message);
                        await dialog.ShowAsync();
                    });
                });
            }

            VideogamesViewModel.LimpiarVideojuegosSeleccionados();
        }
        catch (Exception ex) 
        {
            message = "Error: " + ex;
            dialog.Content = new Dialog(message);
            await dialog.ShowAsync();
        }
    }

    public async Task<string> DescargarVideoJuegoSegundoPlano(string NombreVideojuego)
    {
        await Task.CompletedTask;
        try 
        {
            return await VideogamesViewModel.DescargarVideojuego(NombreVideojuego);
        }
        catch (Exception ex) 
        {
            return "Error: " + ex;
        }
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
                var descargado = cajaVideojuego.FindName("Descargado") as StackPanel;
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
                    var descargado = cajaVideojuego.FindName("Descargado") as StackPanel;
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

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        if (!_globalFunctions.CheckInternetConnectivity())
        {
            var progressRing = FindName("progressRing") as StackPanel;

            if (progressRing != null)
            {
                progressRing.Visibility = Visibility.Collapsed;
            }

            var infoBar = FindName("infoBar") as StackPanel;

            if (infoBar != null)
            {
                infoBar.Visibility = Visibility.Visible;
            }

        }
        else
        {
            var progressRing = FindName("progressRing") as StackPanel;

            if (progressRing != null)
            {
                progressRing.Visibility = Visibility.Visible;
            }
        }
        
    }

    private void GridVideojuegos_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
    {
        if (progressRing != null)
        {
            progressRing.Visibility = Visibility.Collapsed;
        }
        if(infoBar != null)
        {
        
            infoBar.Visibility = Visibility.Collapsed;
        }
    }
}
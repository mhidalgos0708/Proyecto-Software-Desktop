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

namespace Excalinest.Views;

public sealed partial class VideogamesPage : Page
{
    public BitmapImage cover;
    public StackPanel myStackPanel;

    public VideogamesViewModel ViewModel
    {
        get;
    }

    public VideogamesPage()
    {
        ViewModel = App.GetService<VideogamesViewModel>();
        InitializeComponent();

        // taglist.ItemsSource = ViewModel.Tags;
    }



    /*public async void GetPortadas()
    {
        List<BitmapImage> images = new List<BitmapImage>();
        // Assuming you have a list of MongoDB documents named "documents" containing image data as a byte array
        var imageControls = new List<Image>();

        foreach (var document in ViewModel.Source)
        {
            using (var memoryStream = new MemoryStream(document.Portada.Data))
            {
                var bitmapImage = new BitmapImage();
                await bitmapImage.SetSourceAsync(memoryStream.AsRandomAccessStream());

                var imageControl = new Image();
                imageControl.Source = bitmapImage;

                imageControls.Add(imageControl);
                images.Add(bitmapImage);
            }
        }

        // Add the image controls to your UI as needed
        foreach (var imageControl in imageControls)
        {
            myStackPanel.Children.Add(imageControl); // Assumes you have a StackPanel named "myStackPanel" in your UI
            
        }
    }*/

    public async void TagComboBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        ComboBox tag = (ComboBox)sender;
        Tag chosenTag = tag.SelectedItem as Tag;
        Debug.WriteLine(chosenTag.ID);
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

    public async void DescargarVideojuegos(object sender, RoutedEventArgs e)
    {
        await VideogamesViewModel.DescargarVideojuegos();
    }
}
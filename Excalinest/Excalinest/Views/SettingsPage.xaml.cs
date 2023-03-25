using Excalinest.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Core;


namespace Excalinest.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page
{

    private bool _carpetaValida = false;
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
    }

    private async void PickFolderButton_Click(object sender, RoutedEventArgs e)
    {
        // Clear previous returned file name, if it exists, between iterations of this scenario
        PickFolderOutputTextBlock.Text = "";

        // Create a folder picker
        FolderPicker openPicker = new FolderPicker();

        // Retrieve the window handle (HWND) of the current WinUI 3 window.

        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);



        // Initialize the folder picker with the window handle (HWND).
        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

        // Set options for your folder picker
        openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
        openPicker.FileTypeFilter.Add("*");

        // Open the picker for the user to pick a folder
        StorageFolder folder = await openPicker.PickSingleFolderAsync();
        if (folder != null)
        {
            _carpetaValida = true;
            StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
            PickFolderOutputTextBlock.Text = "Carpeta seleccionada: " + folder.Path;
        }
        else
        {
            _carpetaValida = false;
            PickFolderOutputTextBlock.Text = "No se seleccionó ninguna carpeta.";
        }
    }

    private async void ShowDialog_Click(object sender, RoutedEventArgs e)
    {
        ContentDialog dialog = new ContentDialog();

        // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
        dialog.XamlRoot = this.XamlRoot;
        dialog.Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.Title = "Atención";
        dialog.PrimaryButtonText = "Ok";
        dialog.DefaultButton = ContentDialogButton.Primary;


        if (_carpetaValida && NumberBoxMinutos.Value != null)
        {
            string message = "Los cambios se han guardado exitosamente";
            dialog.Content = new SettingsDialogGuardarExitoso(message);
        }
        else
        {
            string message = "Hay campos inválidos";
            dialog.Content = new SettingsDialogGuardarExitoso(message);
        }

        var result = await dialog.ShowAsync(); 
        
    }
}

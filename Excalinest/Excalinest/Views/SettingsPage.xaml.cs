﻿using Excalinest.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage;

using Windows.UI.Core;
using Microsoft.Graph;
using Microsoft.UI.Xaml.Input;
using Windows.System;
using System.Diagnostics;
using Excalinest.Strings;

namespace Excalinest.Views;

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page
{

    private bool _carpetaValida = false;
    private string _carpetaSeleccionada = "";
    public readonly bool isVisible = false;
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();

        GetValues();
    }

    private async void PickFolderButton_Click(object sender, RoutedEventArgs e)
    {
        PickFolderOutputTextBlock.Text = "";

        FolderPicker openPicker = new FolderPicker();

        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);

        WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

        openPicker.SuggestedStartLocation = PickerLocationId.Desktop;
        openPicker.FileTypeFilter.Add("*");

        StorageFolder folder = await openPicker.PickSingleFolderAsync();
        if (folder != null)
        {
            _carpetaValida = true;
            //StorageApplicationPermissions.FutureAccessList.AddOrReplace("PickedFolderToken", folder);
            PickFolderOutputTextBlock.Text = "Carpeta seleccionada: " + folder.Path;
            _carpetaSeleccionada = folder.Path;
        }
        else
        {
            _carpetaValida = false;
            PickFolderOutputTextBlock.Text = "No se seleccionó ninguna carpeta.";
        }
    }

    private async void Guardar_Click(object sender, RoutedEventArgs e)
    {
        ContentDialog dialog = new ContentDialog();

        // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
        dialog.XamlRoot = this.XamlRoot;
        dialog.Style = Microsoft.UI.Xaml.Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.Title = "Atención";
        dialog.PrimaryButtonText = "Ok";
        dialog.DefaultButton = ContentDialogButton.Primary;

        var latestValue = NumberBoxSegundos.Value;

        if (_carpetaValida && NumberBoxSegundos.Value >= 1)
        {
            var guardarExitoso = ViewModel.GuardarDatos(_carpetaSeleccionada, latestValue);
            if (guardarExitoso)
            {
                var message = "Los cambios se han guardado exitosamente";
                dialog.Content = new Dialog(message);
            }
        }
        else
        {
            var message = "Hay campos inválidos";
            dialog.Content = new Dialog(message);
        }

        await dialog.ShowAsync();

    }

    private void GetValues()
    {
        var result = ViewModel.GetValues();
        if (result)
        {
            PickFolderOutputTextBlock.Text = "Carpeta seleccionada: " + ViewModel._rutaArchivo;
            _carpetaSeleccionada = ViewModel._rutaArchivo;
            _carpetaValida = true;
        }
        else
        {
            _carpetaValida = false;
        }
    }

    private void ReloadPage()
    {
        Frame.Navigate(typeof(SettingsPage));
    }
}

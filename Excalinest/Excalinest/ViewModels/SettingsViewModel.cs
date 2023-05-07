using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Excalinest.Contracts.Services;
using Excalinest.Helpers;
using Excalinest.Services;
using Microsoft.UI.Xaml;

using Windows.ApplicationModel;

namespace Excalinest.ViewModels;

public class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;
    private ElementTheme _elementTheme;
    private string _versionDescription;

    private string RutaArchivoConfig = @"C:\Excalinest\VideojuegosExcalinest\config.txt";

    public string _rutaArchivo;
    public int _segundosInactividad;

    ManejoArchivos _manejoArchivos = new ManejoArchivos();

    public ElementTheme ElementTheme
    {
        get => _elementTheme;
        set => SetProperty(ref _elementTheme, value);
    }

    public string VersionDescription
    {
        get => _versionDescription;
        set => SetProperty(ref _versionDescription, value);
    }

    public ICommand SwitchThemeCommand
    {
        get;
    }

    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await _themeSelectorService.SetThemeAsync(param);
                }
            });
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    public void SalirApp(object sender, RoutedEventArgs e)
    {
        Microsoft.UI.Xaml.Application.Current.Exit();
    }

    public bool GetValues()
    {
        bool operacionExitosa = _manejoArchivos.LeerDeArchivoConfig(RutaArchivoConfig);
        if (operacionExitosa)
        {
            _rutaArchivo = _manejoArchivos._rutaArchivoService;
            _segundosInactividad = _manejoArchivos._segundosInactividadService;
            return true;
        }
        else
        {
            return false;
        }
        
    }

    public bool GuardarDatos(string rutaSeleccionada, double tiempoSegundos)
    {
        int segundosInactividad = Convert.ToInt32(tiempoSegundos);
        var contenido = Convert.ToString(tiempoSegundos) + Environment.NewLine + rutaSeleccionada;

        bool operacionExitosa = _manejoArchivos.EscribirEnArchivo(RutaArchivoConfig, contenido);

        return operacionExitosa;
    }

}

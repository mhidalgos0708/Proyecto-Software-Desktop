using System.Diagnostics;
using Excalinest.Strings;
using Excalinest.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;



namespace Excalinest.Views;

public sealed partial class AutenticationPage : Page
{

public AutenticationViewModel ViewModel
    {
        get;
    }

    public AutenticationPage()
    {
        ViewModel = App.GetService<AutenticationViewModel>();
        InitializeComponent();
        HeaderTextBox.Text = "Ingrese la contraseña de administrador";

        CheckButtonsVisibility();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        CheckButtonsVisibility();
    }

    private async void OnAutenticarse(object sender, RoutedEventArgs e)
    {
        ContentDialog dialog = new ContentDialog();

        // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
        dialog.XamlRoot = this.XamlRoot;
        dialog.Style = Microsoft.UI.Xaml.Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.Title = "Atención";
        dialog.PrimaryButtonText = "Aceptar";
        dialog.DefaultButton = ContentDialogButton.Primary;

        if (pwdAdmin.Password != null) {
            if (pwdAdmin.Password == ViewModel.GetPwd())
            {
                dialog.Content = new Dialog("Autenticación exitosa.");
                pwdAdmin.Password = "";
                btnLogIn.Visibility = Visibility.Collapsed;
                btnLogOut.Visibility = Visibility.Visible;

                GlobalVariables.AdminAutenticado = true;

                ReloadPage();
            }
            else
            {
                dialog.Content = new Dialog("Contraseña incorrecta.");
            }
        }
        await dialog.ShowAsync();
    }

    private async void OnLogOut(object sender, RoutedEventArgs e)
    {
        ContentDialog dialog = new ContentDialog();

        // XamlRoot must be set in the case of a ContentDialog running in a Desktop app
        dialog.XamlRoot = this.XamlRoot;
        dialog.Style = Microsoft.UI.Xaml.Application.Current.Resources["DefaultContentDialogStyle"] as Style;
        dialog.Title = "Atención";
        dialog.PrimaryButtonText = "Aceptar";
        dialog.DefaultButton = ContentDialogButton.Primary;

        if (pwdAdmin.Password != null)
        {
            if (pwdAdmin.Password == ViewModel.GetPwd())
            {
                dialog.Content = new Dialog("Cierre de sesión exitoso.");
                pwdAdmin.Password = "";
                btnLogOut.Visibility = Visibility.Collapsed;
                btnLogIn.Visibility = Visibility.Visible;

                GlobalVariables.AdminAutenticado = false;
                
                ReloadPage();
            }
            else
            {
                dialog.Content = new Dialog("Contraseña incorrecta.");
            }
        }
        await dialog.ShowAsync();

    }

    private void ReloadPage()
    {
        Frame.Navigate(typeof(AutenticationPage));
        Debug.WriteLine(GlobalVariables.AdminAutenticado);
    }

    private void CheckButtonsVisibility()
    {
        if (GlobalVariables.AdminAutenticado)
        {
            btnLogIn.Visibility = Visibility.Collapsed;
            btnLogOut.Visibility = Visibility.Visible;
        }
        else
        {
            btnLogIn.Visibility = Visibility.Visible;
            btnLogOut.Visibility = Visibility.Collapsed;
        }
        

    }
}

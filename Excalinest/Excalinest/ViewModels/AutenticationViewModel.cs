using CommunityToolkit.Mvvm.ComponentModel;
using Excalinest.Services;

namespace Excalinest.ViewModels;

public class AutenticationViewModel : ObservableRecipient
{
    ManejoArchivos _manejoArchivos = new ManejoArchivos();

    public AutenticationViewModel()
    {
    }

    public string GetPwd()
    {
        return _manejoArchivos.leerPwd();
    }
}

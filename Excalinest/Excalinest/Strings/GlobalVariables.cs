using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excalinest.Strings;
public class GlobalVariables
{
    private static bool _adminAutenticado;
    public static bool AdminAutenticado
    {
        get => _adminAutenticado;
        set
        {
            if (_adminAutenticado != value)
            {
                _adminAutenticado = value;
                OnStaticPropertyChanged(nameof(AdminAutenticado));
            }
        }
    }

    public GlobalVariables()
    {
        _adminAutenticado=false;
    }

    public static event PropertyChangedEventHandler GlobalPropertyChanged = delegate { };
    public static event EventHandler<MyEventArgs> StaticPropertyChanged;


    public static bool getNoAutenticado()
    {
        return !_adminAutenticado;
    }

    static void OnGlobalPropertyChanged(string propertyName)
    {
        GlobalPropertyChanged(
            typeof(GlobalVariables),
            new PropertyChangedEventArgs(propertyName));
    }

    private static void OnStaticPropertyChanged(string propertyName)
    {
        StaticPropertyChanged?.Invoke(null, new MyEventArgs(propertyName));
    }

}

public class MyEventArgs : EventArgs
{
    public string AdminAutenticado
    {
        get;
    }

    public MyEventArgs(string adminAutenticado)
    {
        AdminAutenticado = adminAutenticado;
    }
}
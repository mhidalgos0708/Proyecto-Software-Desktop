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
                OnGlobalPropertyChanged("adminAutenticado");
            }
        }
    }

    public GlobalVariables()
    {
        _adminAutenticado=false;
    }

    static event PropertyChangedEventHandler GlobalPropertyChanged = delegate { };

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

}

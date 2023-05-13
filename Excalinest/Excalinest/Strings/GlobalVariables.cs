using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excalinest.Strings;
public class GlobalVariables
{
    public static bool adminAutenticado
    {
        get; set;
    }

    public GlobalVariables()
    {
        adminAutenticado = false;
    }
}

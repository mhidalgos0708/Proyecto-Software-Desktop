using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excalinest.Core.Contracts.Services;
using System.Diagnostics;
using Windows.Foundation;

namespace Excalinest.PatronesDiseño.ObserverTiempoInac;


// Clase encargada de botar el proceso si pasa el tiempo de inactividad
public class SubscriberTiempoInac : ISubscriberTiempoInac
{
    private Process Videojuego;

    public SubscriberTiempoInac(Process pVideojuego)
    {
        Videojuego = pVideojuego;
    }

    public void Actualizar()
    {
        Videojuego.Kill();
        Videojuego.Dispose();
    }
}

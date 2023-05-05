using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excalinest.Core.Contracts.Services;
using System.Diagnostics;
using Windows.Foundation;
using Excalinest.Services;

namespace Excalinest.PatronesDiseño.ObserverTiempoInac;


// Clase encargada de botar el proceso si pasa el tiempo de inactividad
public class SubscriberTiempoInac : ISubscriberTiempoInac
{
    private readonly Process Videojuego;

    private DesactivadorComandos _desactivador;

    public SubscriberTiempoInac(string pVideojuego)
    {
        // Inicializar videojuego
        Videojuego = new Process();
        Videojuego.StartInfo.UseShellExecute = false; // Ejecutar directamente desde el archivo ejecutable
        Videojuego.StartInfo.FileName = pVideojuego; // Establecer la ruta del archivo ejecutable
        Videojuego.StartInfo.CreateNoWindow = true; // Abrir una nueva ventana
        Videojuego.Start();

        // Quitar hook del teclado
        //_desactivador = DesactivadorComandos.ObtenerHookTeclado();
        //_desactivador.DesactivarHookTeclado();
    }

    public void Actualizar()
    {
        Videojuego.Kill();
    }

    ~SubscriberTiempoInac()
    {
        Videojuego.Dispose();
        //_desactivador.ActivarHookTeclado();
    }
}

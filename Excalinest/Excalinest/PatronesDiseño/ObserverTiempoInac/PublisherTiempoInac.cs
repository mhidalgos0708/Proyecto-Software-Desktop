using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.UI.Core;
using Timer = System.Timers.Timer; // Eliminar referencia ambigua


namespace Excalinest.PatronesDiseño.ObserverTiempoInac;

public partial class PublisherTiempoInac
{
    private List<ISubscriberTiempoInac> Subscribers; // Lista de suscriptores

    private Timer Temporizador; // Temporizador para contar si el tiempo de inactividad fue superado

    private int SegundosInactividad;

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetLastInputInfo(ref ULTIMOINPUTINFO pUltimoInputInfo);

    [StructLayout(LayoutKind.Sequential)]
    private struct ULTIMOINPUTINFO // Struct para almacenar el tiempo que trancurrió desde el último input del teclado o mouse
    {
        public uint cbSize;
        public uint dwTime;
    }


    public PublisherTiempoInac(int pSegundosInactividad)
    {

        // Inicializar el temporizador
        SegundosInactividad = pSegundosInactividad;
        Subscribers = new List<ISubscriberTiempoInac>();
        Temporizador = new Timer(100); // Establecer los segundos de inactividad a través de un parámetro
        Temporizador.Elapsed += (sender, e) =>
        {
            Notificar(); // Este método se llama em base a la cantidad preconfigurada, en este caso 5 segundos
        };
        Temporizador.AutoReset = true;
        Temporizador.Start();
    }

    // Añadir nuevos suscriptores
    public void Suscribirse(ISubscriberTiempoInac subscriber)
    {
        Subscribers.Add(subscriber);
    }

    private bool HayEntrada()
    {
        var UltimoInputInfo = new ULTIMOINPUTINFO();
        UltimoInputInfo.cbSize = (uint)Marshal.SizeOf(UltimoInputInfo); // Propiedad que almacena el tamaño del struct en bytes
        if (!GetLastInputInfo(ref UltimoInputInfo))
        {
            Temporizador.Stop();
            return false;
        } else
        {
            var tiempoTranscurrido = (uint)Environment.TickCount - UltimoInputInfo.dwTime;
            return tiempoTranscurrido < SegundosInactividad; // Detecta si ha habido actividad dentro del rango permitido de inactividad
        }
    }

    // Eliminar nuevos suscriptores
    public void Desuscribirse(ISubscriberTiempoInac subscriber)
    {
        Subscribers.Remove(subscriber);
    }

    // Notificar que se venció el tiempo de inactividad al pasar cierto tiempo especificado por el usuario admin
    private void Notificar()
    {
        if(!HayEntrada())
        {
            foreach (var subscriber in Subscribers)
            {
                subscriber.Actualizar();
            }
            Temporizador.Stop(); // Parar el temporizador para que no siga ejecutándose en segundo plano
        }
    }

    ~PublisherTiempoInac()
    {
        Temporizador.Dispose();
    }
}

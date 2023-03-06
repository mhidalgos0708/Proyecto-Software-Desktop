using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.UI.Xaml.Controls;
using Control = System.Windows.Forms.Control;
using Timer = System.Timers.Timer; // Eliminar referencia ambigua

namespace Excalinest.PatronesDiseño.ObserverTiempoInac;

public class PublisherTiempoInac
{
    private List<ISubscriberTiempoInac> Subscribers; // Lista de suscriptores

    private Timer Temporizador; // Temporizador para contar si el tiempo de inactividad fue superado


    private int SegundosInactividad;

    public PublisherTiempoInac(int pSegundosInactividad)
    {
        // Inicializar eventos para cancelar la inactividad
        Control ControlEventos = new Control();

        ControlEventos.MouseMove += (sender, e) => ControlarEventosInactividad(sender, e);
        ControlEventos.MouseDown += (sender, e) => ControlarEventosInactividad(sender, e);
        ControlEventos.MouseUp += (sender, e) => ControlarEventosInactividad(sender, e);
        ControlEventos.MouseWheel += (sender, e) => ControlarEventosInactividad(sender, e);
        ControlEventos.KeyDown += (sender, e) => ControlarEventosInactividad(sender, e);

        // Inicializar el temporizador
        SegundosInactividad = pSegundosInactividad;
        Subscribers = new List<ISubscriberTiempoInac>();
        Temporizador = new Timer(SegundosInactividad); // Establecer los segundos de inactividad a través de un parámetro
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

    // Eliminar nuevos suscriptores
    public void Desuscribirse(ISubscriberTiempoInac subscriber)
    {
        Subscribers.Remove(subscriber);
    }

    // Notificar que se venció el tiempo de inactividad al 
    public void Notificar()
    {
        foreach (var subscriber in Subscribers)
        {
            subscriber.Actualizar();
        }
    }

    // Manejar eventos de reseteo de mouse: mover, presionar, liberar click y mover la rueda del mouse. También, manejar interacción con el teclado.
    private void ControlarEventosInactividad(object sender, EventArgs e)
    {
        Temporizador.Stop();
        Temporizador.Start();
    }
}

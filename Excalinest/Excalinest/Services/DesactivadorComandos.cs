using System.Runtime.InteropServices;

using System.Windows.Forms;
using System.Diagnostics;

namespace Excalinest.Services;

internal class DesactivadorComandos
{
    [StructLayout(LayoutKind.Sequential)]
    private struct TCDLLHOOKSTRUCT // Estructura para almacenar los datos de la tecla
    {
        public Keys tecla;
        public int codigoEscaneo;
        public int banderas;
        public int tiempo;
        public IntPtr extra;
    }

    //Funciones usadas para activar y desactivar el Keyboard Hooker

    // Se utiliza en conjunto con SetWindowsHookEx para interceptar eventos del teclado para este contexto
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam); 

    //Función que permite establecer un Keyboard hook para interceptar el uso de ciertas teclas
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int id, LowLevelKeyboardProc callback, IntPtr hMod, uint dwThreadId);

    //Función que desabilita el Keyboard hook
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern bool UnhookWindowsHookEx(IntPtr hook);

    //Método necesario para continuar la cadena de hooks del sistema y no interferir con otras aplicaciones
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hook, int nCode, IntPtr wp, IntPtr lp);

    //Se utiliza para obtener el archivo dll o ejecutable asociado a un modulo 
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string name);

    //Permite saber si una tecla se esta presionando o no, sin recurrir a un manejador de eventos
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern short GetAsyncKeyState(Keys key);


    //Atributos asociadas a la clase
    private static IntPtr ptrHook;
    private static LowLevelKeyboardProc? objProcesoTeclado;

    //Constantes
    private const int WH_KEYBOARD_LL = 13;

    //Singleton para evitar instancias repetidas
    private DesactivadorComandos(){ }

    private static DesactivadorComandos? _instancia;

    public static DesactivadorComandos ObtenerHookTeclado()
    {
        _instancia ??= new DesactivadorComandos(); // ?? significa "instancia no es null"
        return _instancia;
    }

    private const int WM_KEYDOWN = 0x0100;
    private const int LLKHF_ALTDOWN = 0x20;
    private const int LLKHF_EXTENDED = 0x01;

    //nCode: Un entero que indica el tipo de mensaje del teclado. Si es menor que 0 se retorna el valor resultado de invocar a CallNextHookEx
    //wp: La tecla virtual presionada que activo el evento.
    //lp: Puntero a la estructura KBDLLHOOKSTRUCT para manejar la información del evento.
    private IntPtr CapturarTecla(int nCode, IntPtr wp, IntPtr lp)
    {
        if (nCode >= 0)
        {
            if (lp != IntPtr.Zero)
            {
                // Se establece una estructura para administrar los datos de la tecla
                var infoTecla = (TCDLLHOOKSTRUCT)Marshal.PtrToStructure(lp, typeof(TCDLLHOOKSTRUCT));

                // Deshabilitar comandos: Alt + F4, Alt + Esc, Ctrl + Esc, Alt + Tab, botón Windows
                var flags = infoTecla.banderas;
                var altPressed = (flags & LLKHF_ALTDOWN) != 0;

                if (altPressed && infoTecla.tecla == Keys.F4)
                {
                    return (IntPtr)1;
                } else if (altPressed && infoTecla.tecla == Keys.Escape) 
                { 
                    return (IntPtr)1;
                }
                /*
                if (infoTecla.tecla == Keys.RShiftKey || infoTecla.tecla == Keys.ShiftKey || infoTecla.tecla == Keys.Shift || 
                    infoTecla.tecla == Keys.LShiftKey || infoTecla.tecla == Keys.RControlKey || infoTecla.tecla == Keys.LControlKey || 
                    infoTecla.tecla == Keys.F4 || infoTecla.tecla == Keys.LMenu || infoTecla.tecla == Keys.Menu || infoTecla.tecla == Keys.Alt || 
                    infoTecla.tecla == Keys.RMenu || infoTecla.tecla == Keys.Tab || infoTecla.tecla == Keys.Delete || infoTecla.tecla == Keys.RWin || 
                    infoTecla.tecla == Keys.LWin || infoTecla.tecla == Keys.Control || infoTecla.tecla == Keys.ControlKey) 
                {
                    return (IntPtr)1; //Permite marcar el evento como "Handled"
                }*/
            }
        }
        return CallNextHookEx(ptrHook, nCode, wp, lp);
    }

    public void DesactivarHookTeclado()
    {
        UnhookWindowsHookEx(ptrHook); 
    }

    public void ActivarHookTeclado()
    {
        var objModuloActual = Process.GetCurrentProcess().MainModule; //Obtener el proceso actual
        objProcesoTeclado = new LowLevelKeyboardProc(CapturarTecla); //Asociar el hook al método de capturar tecla
        ptrHook = SetWindowsHookEx(WH_KEYBOARD_LL, objProcesoTeclado, GetModuleHandle(objModuloActual.ModuleName), 0); // Establecer hook en modo 13, que permite captar teclas a bajo nivel
    }
}
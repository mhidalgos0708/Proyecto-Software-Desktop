using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Excalinest.Services;


internal class ManejoArchivos
{

    public string _rutaArchivoService;
    public int _segundosInactividadService;

    public bool EscribirEnArchivo(string rutaArchivoConfig, string contenido)
    {
        try
        {
            File.WriteAllText(rutaArchivoConfig, contenido);
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
    }

    public bool LeerDeArchivoConfig(string rutaArchivoConfig)
    {
        try
        {
            string[] lines = File.ReadAllLines(rutaArchivoConfig);

            _segundosInactividadService = int.Parse(lines[0]);
            _rutaArchivoService = lines[1];
            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
    }

    public int leerSegundosInactividad()
    {
        try
        {
            return int.Parse(File.ReadLines(@"C:/Excalinest/VideojuegosExcalinest/config.txt").First());
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return 0;
        }
    }

    public String leerRutaArchivos()
    {
        try
        {
            return File.ReadLines(@"C:/Excalinest/VideojuegosExcalinest/config.txt").Last();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return "";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Excalinest.Services;


internal class ManejoArchivos
{
    public string _archivoConfigRuta = @"C:/Excalinest/VideojuegosExcalinest/config.txt";
    public string _rutaArchivoService;
    public int _segundosInactividadService;

    public ManejoArchivos()
    {
        if (!Directory.Exists(@"C:/Excalinest/"))
        {
            Directory.CreateDirectory(@"C:/Excalinest/");
        }
        if (!Directory.Exists(@"C:/Excalinest/VideojuegosExcalinest"))
        {
            Directory.CreateDirectory(@"C:/Excalinest/VideojuegosExcalinest");
        }
        if (!File.Exists(_archivoConfigRuta))
        {
            using (StreamWriter sw = File.CreateText(_archivoConfigRuta))
            {
                sw.WriteLine(10);
                sw.WriteLine(@"C:/Excalinest/VideojuegosExcalinest/");
            }
        }
    }

    public bool EscribirEnArchivo(string rutaArchivoConfig, string contenido)
    {
        try
        {
            if (File.Exists(rutaArchivoConfig))
            {
                File.WriteAllText(rutaArchivoConfig, contenido);
                return true;
            }
            return false;
            
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
        return int.Parse(File.ReadLines(_archivoConfigRuta).First());
    }

    public String leerRutaArchivos()
    {
        return File.ReadLines(_archivoConfigRuta).Last();
    }
}

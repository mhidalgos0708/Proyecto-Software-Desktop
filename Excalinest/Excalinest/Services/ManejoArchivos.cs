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
            if(File.Exists(rutaArchivoConfig))
            {
                string[] lines = File.ReadAllLines(rutaArchivoConfig);

                _segundosInactividadService = int.Parse(lines[0]);
                _rutaArchivoService = lines[1];
                return true;
            }

            else
            {
                _segundosInactividadService = 0;
                _rutaArchivoService = "";
                return false;
            }
            
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
            if (File.Exists(_archivoConfigRuta))
            {
                return int.Parse(File.ReadLines(_archivoConfigRuta).First());
            }
            else
            {
                using (StreamWriter sw = File.CreateText(_archivoConfigRuta))
                {
                    sw.WriteLine(10);
                    sw.WriteLine(@"C:/Excalinest/VideojuegosExcalinest/");
                }
                return 0;
            }
            
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
            if (File.Exists(_archivoConfigRuta))
            {
                return File.ReadLines(_archivoConfigRuta).Last();
            }
            else
            {
                using (StreamWriter sw = File.CreateText(_archivoConfigRuta))
                {
                    sw.WriteLine(10);
                    sw.WriteLine(@"C:/Excalinest/VideojuegosExcalinest/");
                }
                return "";
            }
            
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return "";
        }
    }
}

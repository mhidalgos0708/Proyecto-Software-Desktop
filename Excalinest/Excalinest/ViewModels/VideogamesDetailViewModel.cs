﻿using System.Diagnostics;
using System.Windows.Forms;
using CommunityToolkit.Mvvm.ComponentModel;

using Excalinest.Contracts.ViewModels;
using Excalinest.Core.Contracts.Services;
using Excalinest.Core.Models;
using Excalinest.Core.Services;
using Excalinest.PatronesDiseño.ObserverTiempoInac;
using Excalinest.Services;
using Excalinest.Strings;

namespace Excalinest.ViewModels;
using Tag = Excalinest.Core.Models.Tag;

public class VideogamesDetailViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;
    private static ServicioVideojuego? servicioVideojuego;
    private static ServicioVideojuegoEtiqueta _servicioVideojuegoEtiqueta;
    private Videojuego? _item;
    public List<Tag> _listaEtiquetas;

    private static PublisherTiempoInac? NotificadorTiempoInac;
    private static ISubscriberTiempoInac? ObservadorTiempoInac;

    private static string NombreVideojuego = "";

    private static int SegundosInactividad = 0;
    private static string RutaJuego = "";

    private GlobalFunctions _globalFunctions;

    ManejoArchivos _manejoArchivos = new ManejoArchivos();

    public Videojuego? Item
    {
        get => _item;
        set => SetProperty(ref _item, value);
    }

    public VideogamesDetailViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
        servicioVideojuego = new ServicioVideojuego(new MongoConnection());
        SegundosInactividad = _manejoArchivos.leerSegundosInactividad();
        _servicioVideojuegoEtiqueta = new ServicioVideojuegoEtiqueta(new MongoConnection());

        _globalFunctions = new GlobalFunctions();
        _listaEtiquetas = new List<Tag>();
    }

    public async void OnNavigatedTo(object parameter)
    {
        RutaJuego = _manejoArchivos.leerRutaArchivos();
        _listaEtiquetas.Clear();

        if (_globalFunctions.CheckInternetConnectivity())
        {
            
            if (parameter is string titulo)
            {
                if (servicioVideojuego != null)
                {
                    Item = await servicioVideojuego.GetVideojuegoPorTitulo(titulo);
                    NombreVideojuego = Item.Titulo;

                    var data = await _servicioVideojuegoEtiqueta.GetEtiquetasByVideojuego(Item.ID);
                    foreach (var item in data)
                    {
                        _listaEtiquetas.Add(item);
                    }
                }
                
            }
        }
        else
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var defaultImagePath = Path.Combine(baseDirectory, "Assets", "default.jpg");
            var defaultImageBytes = File.ReadAllBytes(defaultImagePath);

            if (parameter is string titulo)
            {
                NombreVideojuego = titulo;
                if (Directory.Exists(RutaJuego + titulo))
                {
                    var videojuego = new Videojuego
                    {
                        Titulo = titulo,
                        Portada = new ImageMongo
                        {
                            ImgType = "image/jpg",
                            Data = defaultImageBytes
                        },
                        Facebook = new ImageMongo(),
                        Instagram = new ImageMongo(),
                        Twitter = new ImageMongo(),
                        Sinopsis = "Desconocido",
                        Usuario = "Desconocido",
                        bucketId = "Desconocido",
                        Etiquetas = new List<Tag>()
                    };

                    Item = videojuego;
                }

                else
                {

                }
                
            }
        }
        
    }

    public void OnNavigatedFrom()
    {
    }

    // Método para ejecutar el videojuego actual, busca en la ruta del mismo nombre el ejecutable correspondiente
    public static void EjecutarVideojuego()
    {

        // Esta línea se utiliza debido a que las rutas relativas en c# se establecen desde system32
        // Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory); // Establece a CurrentDirectory la ruta donde se buscan los archivos ensamblador y se encuentran los videojuegos ejecutables

        try
        {
            // Obtener los nombres de archivos ejecutables dentro de la carpeta del juego actual
            var VideojuegoEjecutable = Directory.GetFiles(RutaJuego + NombreVideojuego, "*.exe", SearchOption.AllDirectories) // Retorna una lista de archivos .exe dentro de la carpeta RutaJuego
                    .Where(archivo => !archivo.Contains("UnityCrashHandler"))
                    .AsEnumerable()
                    .ToArray();

            if(VideojuegoEjecutable.Length > 0)
            {
                NotificadorTiempoInac = new PublisherTiempoInac(SegundosInactividad * 1000);
                ObservadorTiempoInac = new SubscriberTiempoInac(VideojuegoEjecutable[0]);
                NotificadorTiempoInac.Suscribirse(ObservadorTiempoInac);
            }
            else
            {
                throw new Exception("El archivo zip proporcionado no posee un videojuego ejecutable");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public static bool EliminarVideojuego()
    {
        try
        {
            if (Directory.Exists(RutaJuego + NombreVideojuego))
            {
                var files = Directory.GetFiles(RutaJuego + NombreVideojuego);
                var subdirectories = Directory.GetDirectories(RutaJuego + NombreVideojuego);

                foreach (var file in files)
                {
                    File.Delete(file);
                    while (File.Exists(file))
                    {
                        Thread.Sleep(100);
                    }
                }

                foreach (var subdirectory in subdirectories)
                {
                    Directory.Delete(subdirectory, true);
                    while (Directory.Exists(subdirectory))
                    {
                        Thread.Sleep(100);
                    }
                }

                Directory.Delete(RutaJuego + NombreVideojuego);

                while (Directory.Exists(RutaJuego + NombreVideojuego))
                {
                    Thread.Sleep(100);
                }

                return true;
            }
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public static bool EsVideojuegoDescargado()
    {
        return Directory.Exists(RutaJuego + NombreVideojuego);
    }

    public static async Task<string> DescargarVideojuego()
    {
        await Task.CompletedTask;
        if (servicioVideojuego != null)
        {
            return await servicioVideojuego.DownloadVideojuego(RutaJuego, NombreVideojuego + ".zip");
        }
        return "Error al descargar el videojuego.";
    } 
}

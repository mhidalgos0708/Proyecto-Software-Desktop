﻿using System.IO;
using System.IO.Compression;
using Excalinest.Core.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Tag = Excalinest.Core.Models.Tag;

namespace Excalinest.Core.Services;

public class ServicioVideojuego
{
    public readonly IMongoDatabase database;
    private readonly IMongoCollection<Videojuego> collection;

    public ServicioVideojuego(MongoConnection mongoConnection)
    {
        database = mongoConnection.database;
        collection = database.GetCollection<Videojuego>("videogames");
    }

    
    public async Task<IEnumerable<Videojuego>> GetVideojuegos()
    {
        await Task.CompletedTask;
        var filter = Builders<Videojuego>.Filter.Empty;
        return collection.Find(filter).ToList();
    }

    public async Task<IEnumerable<Videojuego>> GetVideojuegosByTagID(int ID)
    {
        await Task.CompletedTask;
        var filter1 = Builders<Videojuego>.Filter.Eq("tags.id", ID);
        return collection.Find(filter1).ToList();
    }

    public async Task<Videojuego> GetVideojuego(string id)
    {
        await Task.CompletedTask;
        var filter_id = Builders<Videojuego>.Filter.Eq("id", ObjectId.Parse(id));
        var entity = collection.Find(filter_id).FirstOrDefault();
        return entity;
    }

    public async Task<Videojuego> GetVideojuegoPorTitulo(string titulo)
    {
        await Task.CompletedTask;
        var filter = Builders<Videojuego>.Filter.Eq("titulo", titulo);
        var entity = collection.Find(filter).FirstOrDefault();
        if (entity == null)
        {
            var path = @"C:\Excalinest\VideojuegosExcalinest\" + titulo;
            if (Directory.Exists(path))
            {
                var defaultImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "default.jpg");
                var defaultImageBytes = File.ReadAllBytes(defaultImagePath);

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

                return videojuego;
            }
        }
        return entity;
    }

    public async Task<string> DownloadVideojuego(string ruta, string nombreArchivo)
    {
        await Task.CompletedTask;

        var options = new GridFSBucketOptions
        {
            BucketName = "builds"
        };

        var bucket = new GridFSBucket(database, options);
        try
        {
            var fileStream = bucket.OpenDownloadStreamByName(nombreArchivo);
            try
            {
                if (Directory.Exists(ruta + nombreArchivo[..^4]))
                {
                    return "Videojuego " + nombreArchivo[..^4] + " ya fue descargado previamente";
                }
                if (File.Exists(ruta + nombreArchivo))
                {
                    File.Delete(ruta + nombreArchivo);
                }
                using var localFileStream = new FileStream(ruta + nombreArchivo, FileMode.CreateNew);
                fileStream.CopyTo(localFileStream);
                fileStream.Close();
                localFileStream.Close();

                // Descomprimir y borrar zip
                ZipFile.ExtractToDirectory(ruta + nombreArchivo, ruta + nombreArchivo[..^4]);
                File.Delete(ruta + nombreArchivo);
                return "Videojuego " + nombreArchivo[..^4] + " descargado en ruta " + ruta;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}

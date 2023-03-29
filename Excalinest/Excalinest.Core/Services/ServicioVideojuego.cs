using System.IO;
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
    private readonly IMongoCollection<Tag> collection_tags;

    public ServicioVideojuego(MongoConnection mongoConnection)
    {
        database = mongoConnection.database;
        collection = database.GetCollection<Videojuego>("videogames");
        collection_tags = database.GetCollection<Tag>("tags");
    }

    public async Task<IEnumerable<Tag>> GetTags()
    {
        await Task.CompletedTask;
        var filter = Builders<Tag>.Filter.Empty;
        return collection_tags.Find(filter).ToList();

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
                using var localFileStream = new FileStream(ruta + nombreArchivo, FileMode.CreateNew);
                fileStream.CopyTo(localFileStream);
                fileStream.Close();
                localFileStream.Close();
                return "Videojuego descargado en ruta " + ruta;
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

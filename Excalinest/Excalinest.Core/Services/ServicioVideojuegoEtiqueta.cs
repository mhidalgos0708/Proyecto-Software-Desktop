using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.IO.Compression;
using Excalinest.Core.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using VideojuegoEtiqueta = Excalinest.Core.Models.VideojuegoEtiqueta;
using Tag = Excalinest.Core.Models.Tag;
using System.Diagnostics;

namespace Excalinest.Core.Services;
public class ServicioVideojuegoEtiqueta
{

    public readonly IMongoDatabase database;
    private readonly IMongoCollection<VideojuegoEtiqueta> collection;
    private readonly IMongoCollection<Videojuego> collection_videojuegos;
    private readonly IMongoCollection<Tag> collection_tags;

    public ServicioVideojuegoEtiqueta(MongoConnection mongoConnection)
    {
        database = mongoConnection.database;
        collection = database.GetCollection<VideojuegoEtiqueta>("videogame-tags");
        collection_videojuegos = database.GetCollection<Videojuego>("videogames");
        collection_tags = database.GetCollection<Tag>("tags");
    }

    public async Task<IEnumerable<Videojuego>> GetVideojuegosByTagId(int tagId)
    {
        await Task.CompletedTask;
        var filter = Builders<VideojuegoEtiqueta>.Filter.Eq("tag", tagId);
        var list = collection.Find(filter).ToList();

        var newList = list.Select(x => ObjectId.Parse(x.Videojuego)).ToList();

        var filter1 = Builders<Videojuego>.Filter.In("_id", newList);
        return collection_videojuegos.Find(filter1).ToList();
    }

    public async Task<IEnumerable<Tag>> GetEtiquetasByVideojuego(string ID_videojuego)
    {
        await Task.CompletedTask;
        var filter = Builders<VideojuegoEtiqueta>.Filter.Eq("videogame",ID_videojuego);
        var list = collection.Find(filter).ToList();
    
        var newListTagsOnly = list.Select(x => x.Etiqueta).ToList();

        var filter1 = Builders<Tag>.Filter.In("id", newListTagsOnly);

        var resultList = collection_tags.Find(filter1).ToList();

        return resultList;
    }  
}

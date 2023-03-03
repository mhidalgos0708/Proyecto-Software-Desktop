using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excalinest.Core.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Diagnostics;
using Microsoft.Graph.ExternalConnectors;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Linq;
using System.Text.Json;

namespace Excalinest.Core.Services;
public class ServicioVideojuego
{
    private readonly MongoConnection _mongoConnection;
    private readonly IMongoCollection<Videojuego> _videojuegos;

    public readonly MongoClientSettings settings;
    public readonly MongoClient client;
    public readonly IMongoDatabase database;

    IMongoCollection<Videojuego> collection;

    public ServicioVideojuego()
    {
        settings = MongoClientSettings.FromConnectionString("mongodb+srv://excalinest:AcWqA5Ez6LNGUiKF@excalinestcluster.auytmua.mongodb.net/?retryWrites=true&w=majority");
        client = new MongoClient(settings);
        database = client.GetDatabase("ExcalinestDB");


        _mongoConnection = new MongoConnection();
        _videojuegos = _mongoConnection.database.GetCollection<Videojuego>("videogames");
        collection = database.GetCollection<Videojuego>("videogames");
    }

    public async Task<IEnumerable<Videojuego>> GetVideojuegos()
    {
        await Task.CompletedTask;
        Debug.WriteLine("get video juegos");

        /*var dbList = client.ListDatabases().ToList();

        var list = new List<Object>();
        foreach (var item in dbList)
        {
            var model = BsonSerializer.Deserialize<Object>(item);
            list.Add(model);
        }*/

        IMongoCollection<BsonDocument> collection1 = database.GetCollection<BsonDocument>("videogames");

        //collection.FindSync(x => x.titulo == "Rat Game 2").ToList().ForEach(game => Debug.WriteLine("JUEGO ", game));


        //Debug.WriteLine(database.GetCollection<BsonDocument>("videogames").ToString());}

        var filterBSON = Builders<BsonDocument>.Filter.Empty;
        var listAll = collection1.Find(filterBSON).ToList();


        Debug.WriteLine("todos videojuegos: ", listAll.ToString());

        var filter = Builders<Videojuego>.Filter.Empty;
        return collection.Find(filter).ToList();
    }

    public async Task<Videojuego> GetVideojuego(string id)
    {
        var filter_id = Builders<Videojuego>.Filter.Eq("id", ObjectId.Parse(id));
        var entity = collection.Find(filter_id).FirstOrDefault();
        return entity;
    }

    public async Task<Videojuego> CreateVideojuego(Videojuego videojuego)
    {
    
           await collection.InsertOneAsync(videojuego);
           return videojuego;
       }

    public async Task UpdateVideojuego(ObjectId id, Videojuego videojuegoIn)
    {
    
           await collection.ReplaceOneAsync(videojuego => videojuego.ID == id, videojuegoIn);
       }

    public async Task RemoveVideojuego(Videojuego videojuegoIn)
    {
    
           await collection.DeleteOneAsync(videojuego => videojuego.ID == videojuegoIn.ID);
       }

    public async Task RemoveVideojuego(ObjectId id)
    {
    
           await collection.DeleteOneAsync(videojuego => videojuego.ID == id);
       }
}

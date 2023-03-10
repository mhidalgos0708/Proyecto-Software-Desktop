using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excalinest.Core.Models;
using MongoDB.Bson;
using MongoDB.Driver;

using Microsoft.Graph.ExternalConnectors;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Linq;


using System.Text.Json;
using Tag = Excalinest.Core.Models.Tag;
using Microsoft.Graph;

namespace Excalinest.Core.Services;
public class ServicioVideojuego
{
    private readonly MongoConnection _mongoConnection;
    //private readonly IMongoCollection<Videojuego> _videojuegos;

    public readonly MongoClientSettings settings;
    public readonly MongoClient client;
    public readonly IMongoDatabase database;

    IMongoCollection<Videojuego> collection;
    IMongoCollection<Tag> collection_tags;

    public ServicioVideojuego()
    {
        settings = MongoClientSettings.FromConnectionString("mongodb+srv://excalinest:AcWqA5Ez6LNGUiKF@excalinestcluster.auytmua.mongodb.net/?retryWrites=true&w=majority"); client = new MongoClient(settings);
        database = client.GetDatabase("ExcalinestDB");


        _mongoConnection = new MongoConnection();
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
        var filter_id = Builders<Videojuego>.Filter.Eq("id", ObjectId.Parse(id));
        var entity = collection.Find(filter_id).FirstOrDefault();
        return entity;
    }

    public async Task<Videojuego> GetVideojuegoPorTitulo(string titulo)
    {
        var filter = Builders<Videojuego>.Filter.Eq("titulo", titulo);
        var entity = collection.Find(filter).FirstOrDefault();
        return entity;
    }

    public async Task<Videojuego> CreateVideojuego(Videojuego videojuego)
    {
    
           await collection.InsertOneAsync(videojuego);
           return videojuego;
       }

    public async Task UpdateVideojuego(String id, Videojuego videojuegoIn)
    {
    
           await collection.ReplaceOneAsync(videojuego => videojuego.ID == id, videojuegoIn);
       }

    public async Task RemoveVideojuego(Videojuego videojuegoIn)
    {
    
           await collection.DeleteOneAsync(videojuego => videojuego.ID == videojuegoIn.ID);
       }

    public async Task RemoveVideojuego(String id)
    {
    
           await collection.DeleteOneAsync(videojuego => videojuego.ID == id);
       }
}

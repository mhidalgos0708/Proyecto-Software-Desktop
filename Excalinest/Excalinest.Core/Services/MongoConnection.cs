using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Excalinest.Core.Services;

public class MongoConnection
{
    private readonly MongoClient _mongoclient;
    private string CurrentUrl
    {
        get; set;
    }

    public readonly MongoClientSettings settings;
    public readonly MongoClient client;
    public readonly IMongoDatabase database;

    // Conexión a mongo funcionando correctamente
    public MongoConnection()
    {
        settings = MongoClientSettings.FromConnectionString("mongodb+srv://excalinest:AcWqA5Ez6LNGUiKF@excalinestcluster.auytmua.mongodb.net/?retryWrites=true&w=majority");
        client = new MongoClient(settings);
        database = client.GetDatabase("ExcalinestDB");
    }

    //Métodos incompletos
    public async Task<IEnumerable<string>> GetCollectionsFrom()
    {
        var cursor = await database.ListCollectionNamesAsync();
        return cursor.ToEnumerable();
    }

    public async Task<IEnumerable<string>> GetDatabases()
    {
        var cursor = await _mongoclient.ListDatabaseNamesAsync();
        return cursor.ToEnumerable();
    }
}
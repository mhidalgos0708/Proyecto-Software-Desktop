using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Excalinest.Core.Services;

public class MongoConnection
{
    private MongoClient _mongoclient;
    public string CurrentUrl
    {
        get; private set;
    }

    public MongoConnection()
    {
        CurrentUrl = "mongodb://excalinest:<AcWqA5Ez6LNGUiKF>@ac-7lxzxoi-shard-00-00.auytmua.mongodb.net:27017,ac-7lxzxoi-shard-00-01.auytmua.mongodb.net:27017,ac-7lxzxoi-shard-00-02.auytmua.mongodb.net:27017/?ssl=true&replicaSet=atlas-131utc-shard-0&authSource=admin&retryWrites=true&w=majority";
        _mongoclient = new MongoClient(CurrentUrl);
    }

    public async Task<IEnumerable<string>> GetCollectionsFrom(string dbName)
    {
        var database = _mongoclient.GetDatabase(dbName);
        var cursor = await database.ListCollectionNamesAsync();
        return cursor.ToEnumerable();
    }

    public async Task<IEnumerable<string>> GetDatabases()
    {
        var cursor = await _mongoclient.ListDatabaseNamesAsync();
        return cursor.ToEnumerable();
    }

    public async Task<(BsonElement, BsonElement)> ExecuteRawAsync(string dbName, string command)
    {
        var db = _mongoclient.GetDatabase(dbName);
        var result = await db.RunCommandAsync<BsonDocument>(command);
        result.TryGetElement("cursor", out BsonElement cursor);
        result.TryGetElement("ok", out BsonElement ok);
        return (cursor, ok);
    }
}
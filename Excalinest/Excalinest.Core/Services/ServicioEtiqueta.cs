using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Excalinest.Core.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Tag = Excalinest.Core.Models.Tag;


namespace Excalinest.Core.Services;
public class ServicioEtiqueta
{
    public readonly IMongoDatabase database;
    private readonly IMongoCollection<Tag> collection_tags;

    public ServicioEtiqueta(MongoConnection mongoConnection)
    {
        database = mongoConnection.database;
        collection_tags = database.GetCollection<Tag>("tags");
    }

    public async Task<IEnumerable<Tag>> GetTags()
    {
        await Task.CompletedTask;
        var filter = Builders<Tag>.Filter.Empty;
        return collection_tags.Find(filter).ToList();

    }
}

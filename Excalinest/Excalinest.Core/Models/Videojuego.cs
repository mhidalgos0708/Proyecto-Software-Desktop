using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Excalinest.Core.Models;

// Modelo para almacenar los videojuegos extraídos de la base de datos de Mongo
[BsonIgnoreExtraElements]
public class Videojuego
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    
    public string? ID
    {
        get; set;
    }

    [BsonElement("titulo")]
    public string Titulo { get; set; } = null!;

    [BsonElement("portada")]
    public Portada Portada { get; set; } = null!;

    [BsonElement("facebook")]
    public Portada Facebook { get; set; } = null!;

    [BsonElement("instagram")]
    public Portada Instagram { get; set; } = null!;

    [BsonElement("twitter")]
    public Portada Twitter { get; set; } = null!;

    [BsonElement("sinopsis")]
    public string Sinopsis { get; set; } = null!;

    [BsonElement("usuario")]
    public string Usuario { get; set; } = null!;

    [BsonElement("bucketId")]
    public string bucketId { get; set; } = null!;

    [BsonElement("tags")]
    public List<Tag> Etiquetas { get; set; } = null!;
    
}

[BsonIgnoreExtraElements]
public class Tag{
    [BsonElement("name")]
    public string Nombre { get; set; } = null!;

    [BsonElement("id")]
    public int ID { get; set; }

}

public class Portada
{
    [BsonElement("tipoImagen")]
    public string ImgType { get; set; } = null;

    [BsonElement("data")]
    public byte[] Data { get; set; } = null;

}

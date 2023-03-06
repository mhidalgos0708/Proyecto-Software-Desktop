using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Excalinest.Core.Models;


// Modelo para almacenar los videojuegos extraídos de la base de datos de Mongo
[BsonIgnoreExtraElements]
public class Videojuego
{
    //[BsonId]
    //[BsonRepresentation(BsonType.ObjectId)]
    public ObjectId ID
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

    [BsonElement("juegoZip")]
    public Zip JuegoZIP { get; set; } = null!;

    [BsonElement("tags")]
    public List<string> Etiquetas { get; set; } = null!;
    /*
    public List<Image> RedesSociales { get; set; } = null!;*/
}

public class Portada
{
    [BsonElement("tipoImagen")]
    public string ImgType { get; set; } = null;

    [BsonElement("data")]
    public byte[] Data { get; set; } = null;

}

public class Zip
{
    [BsonElement("tipoArchivo")]
    public string FileType { get; set; } = null;

    [BsonElement("data")]
    public byte[] Data { get; set; } = null;

}

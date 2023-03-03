using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Drawing;

namespace Excalinest.Core.Models;

// Modelo para almacenar los videojuegos extraídos de la base de datos de Mongo
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

    public Image Portada { get; set; } = null!;

    public string Sinopsis { get; set; } = null!;

    public string Usuario { get; set; } = null!;

    public FileStream JuegoZIP { get; set; } = null!;

    public List<string> Etiquetas { get; set; } = null!;

    public List<Image> RedesSociales { get; set; } = null!;
}

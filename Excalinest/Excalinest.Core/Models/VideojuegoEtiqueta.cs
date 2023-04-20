using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace Excalinest.Core.Models;

[BsonIgnoreExtraElements]
public class VideojuegoEtiqueta
{
    [BsonElement("videogame")]
    public string Videojuego { get; set; } = null!;

    [BsonElement("tag")]
    public int Etiqueta { get; set; } = 0!;
}

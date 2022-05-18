using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ShopifyChallengeBackEndApi.Models;

[BsonIgnoreExtraElements]
public class Recycle
{
    [BsonId] // Annotated with BsonId to make this property the document's primary key
    [BsonRepresentation(BsonType.ObjectId)] // to allow passing the parameter as type string instead of an ObjectId structure
    public string? recycle_id { get; set; } = null!;

    public string name { get; set; } = null!;
    public string comment { get; set; } = null!;

    public string date { get; set; } = null!;

}
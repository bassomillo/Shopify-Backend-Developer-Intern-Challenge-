using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ShopifyChallengeBackEndApi.Models;

[BsonIgnoreExtraElements]
public class Inventory
{
    [BsonId] // Annotated with BsonId to make this property the document's primary key
    [BsonRepresentation(BsonType.ObjectId)] // to allow passing the parameter as type string instead of an ObjectId structure
    public string? product_id { get; set; } = null!;

    public string category { get; set; } = null!;

    public string description { get; set; } = null!;

    public string name { get; set; } = null!;

    public int? price { get; set; } = null!;

    public int? quantity { get; set; } = null!;

}
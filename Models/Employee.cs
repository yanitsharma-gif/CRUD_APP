
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace Practice_3._0.Models;
public class Employee
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;
}

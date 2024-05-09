using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WholeSaler.Web.Areas.Auth.Models.ViewModels.Product
{
    public class ProductImage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? FileName { get; set; }
        public string? Path { get; set; }
    }
}

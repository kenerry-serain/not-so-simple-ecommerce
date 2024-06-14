using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NotSoSimpleEcommerce.Main.Domain.Models
{
    public class ReportEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("productId")]
        public int ProductId { get; set; }

        [BsonElement("productName")]
        public string ProductName { get; set; }

        [BsonElement("totalOrders")]
        public int TotalOrders { get; set; }

        [BsonElement("totalOrdered")]
        public int TotalOrdered{ get; set; }

        [BsonElement("totalSold")]
        public decimal TotalSold{ get; set; }
    }
}

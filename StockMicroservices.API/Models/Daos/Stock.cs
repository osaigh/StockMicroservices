using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StockMicroservices.API.Models.Daos
{
    public class Stock
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public double Price { get; set; }

        public int Volume { get; set; }

        public List<StockHistory> StockHistories { get; set; } = new List<StockHistory>();
    }
}

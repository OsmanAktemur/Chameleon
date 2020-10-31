using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Chameleon.Entities
{
    [BsonIgnoreExtraElements]
    public class ConfigEntity<T>
    {
        
        
        public ObjectId Id { get; set; }
        public DateTime UpdatedDate { get; set; }
         public T Config { get; set; }
    }
}
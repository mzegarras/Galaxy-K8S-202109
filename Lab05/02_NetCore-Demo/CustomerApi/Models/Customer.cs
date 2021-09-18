using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CustomerApi.Models
{
    public class Customer{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id{get;set;}
        public string FirstName{get;set;}
        public string LastName{get;set;}
    }
}
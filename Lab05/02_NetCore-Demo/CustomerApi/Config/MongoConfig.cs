     using System;

namespace CustomerApi.Config
{

    public interface IMongoConfig{
         public string CollectionName{get;set;}
        public string ConnectionString{get;set;}
        public string DatabaseName{get;set;}
    }
    public class MongoConfig : IMongoConfig
    {

        public string CollectionName{get;set;}
        public string ConnectionString{get;set;}
        public string DatabaseName{get;set;}

    }
      
}
 
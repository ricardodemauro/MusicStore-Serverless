using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.CoreFunctions
{
    public static class MongoCollectionFactory
    {
        public static IMongoCollection<T> Get<T>(string collectionName)
        {
            var connString = Environment.GetEnvironmentVariable("ServerlessDbConnectionString");
            var client = new MongoClient(connString);

            var databaseName = Environment.GetEnvironmentVariable("ServerlessDbName");
            var db = client.GetDatabase(databaseName);

            return db.GetCollection<T>(collectionName);
        }
    }
}

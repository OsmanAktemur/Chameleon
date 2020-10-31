using System;
using System.Threading;
using System.Threading.Tasks;
using Chameleon.Contracts;
using Chameleon.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using Chameleon.Utils;
using MongoDB.Bson.Serialization.Conventions;

namespace Chameleon
{
    public class MongoDbConfigReader<T> : IConfigReader<T>
    {
        private ConfigEntity<T> config;
 
        private IMongoDatabase database;
        private IMongoCollection<ConfigEntity<T>> collection;


        public MongoDbConfigReader(string mongoDbConnectionString, string configDbName, string configCollectionName)
        {
            
            var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
            ConventionRegistry.Register("IgnoreExtraElements", conventionPack, type => true);
            
            
            IMongoClient mongoClient;

            mongoClient = new MongoClient(mongoDbConnectionString);
            database = mongoClient.GetDatabase(configDbName);
            collection = database.GetCollection<ConfigEntity<T>>(configCollectionName);

            
  
            
            InitialConfigs().GetAwaiter().GetResult();

 
            Watch();
        }


        private async Task InitialConfigs()
        {
            var allConfigDocumentsCursor = await collection.FindAsync(new BsonDocument());
            var allConfigDocuments = allConfigDocumentsCursor.ToList();

            if (!allConfigDocuments.Any())
            {
                var initialConfig = new ConfigEntity<T>()
                {
                    UpdatedDate = DateTime.UtcNow,
                };

                await collection.InsertOneAsync(initialConfig);

                this.config = initialConfig;
            }

            if (allConfigDocuments.Count() != 1)
            {
                throw new Exception(
                    "Config collection document count must be only '1'. Please delete other config documents");
            }

            this.config = allConfigDocuments.FirstOrDefault();
 
        }

        private async Task Watch()
        {
            CancellationToken stoppingToken = new CancellationToken();

            await Task.Factory.StartNew(async () =>
                {
                    using (var cursor = await collection.WatchAsync())
                    {
                        foreach (var change in cursor.ToEnumerable())
                        {
                           // Console.WriteLine("degistii");
                            // await collection.UpdateOneAsync(x => x.Id == this.config.Id,
                            //     new UpdateDefinitionBuilder<ConfigEntity<T>>()
                            //         .Set(p => p.UpdatedDate, DateTime.UtcNow));

                            
                            //this.config.Config = change.FullDocument.Config;
 
                            
                            PropertyCopier<T,T>.Copy(change.FullDocument.Config,this.config.Config);

                             
                        }
                    }
                }
                , stoppingToken);
        }


        public T GetConfig()
        {
            return this.config.Config;
        }
    }
}
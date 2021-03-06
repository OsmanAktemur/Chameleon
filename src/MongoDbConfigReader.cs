﻿using System;
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
    public class MongoDbConfigReader<T> : IConfigReader<T> where T : new()
    {
        private ConfigEntity<T> config;

        private IMongoDatabase database;
        private IMongoCollection<ConfigEntity<T>> collection;
        private IMongoClient mongoClient;


        public static MongoDbConfigReader<T> Create(
            string mongoDbConnectionString,
            string configDbName,
            string configCollectionName,
            bool enabledAutoRefresh = true)
        {
            var reader = new MongoDbConfigReader<T>(mongoDbConnectionString, configDbName, configCollectionName);
            
            reader.InitialConfigs();

            if (enabledAutoRefresh)
            {
                reader.StartWatching();
            }

            return reader;
        }

        private MongoDbConfigReader(string mongoDbConnectionString, string configDbName, string configCollectionName)
        {
            var conventionPack = new ConventionPack {new IgnoreExtraElementsConvention(true)};
            ConventionRegistry.Register("IgnoreExtraElements", conventionPack, type => true);


            mongoClient = new MongoClient(mongoDbConnectionString);
            database = mongoClient.GetDatabase(configDbName);
            collection = database.GetCollection<ConfigEntity<T>>(configCollectionName);
        }


        private  void InitialConfigs()
        {
            var allConfigDocumentsCursor = collection.Find(new BsonDocument());
            var allConfigDocuments = allConfigDocumentsCursor.ToList();

            if (!allConfigDocuments.Any())
            {
                var initialConfig = new ConfigEntity<T>()
                {
                    UpdatedDate = DateTime.UtcNow,
                    Config = new T()
                };

                collection.InsertOne(initialConfig);

                this.config = initialConfig;

                return;
            }

            if (allConfigDocuments.Count() != 1)
            {
                throw new Exception(
                    "Config collection document count must be only '1'. Please delete other config documents");
            }
            this.config = allConfigDocuments.FirstOrDefault();
        }

        private Task StartWatching()
        {
           return Task.Factory.StartNew(async () =>
            {
                using (var cursor = await collection.WatchAsync())
                {
                    foreach (var change in cursor.ToEnumerable())
                    {
                        PropertyCopier<T, T>.Copy(change.FullDocument.Config, this.config.Config);
                    }
                }
            });
        }


        public T GetConfig()
        {
            return this.config.Config;
        }
    }
}
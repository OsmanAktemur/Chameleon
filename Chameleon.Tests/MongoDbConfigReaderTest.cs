using System;
using System.Threading;
using MongoDB.Bson;
using NUnit.Framework;

namespace Chameleon.Tests
{
    public class TestConfig
    {
        public string ProjectName { get; set; }
    }

    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestConfigs()
        {
            MongoDbConfigReader<TestConfig> reader = new MongoDbConfigReader<TestConfig>(
                "",
                "Configs",
                "EndorAPIConfigs"
            );




            var x = reader.GetConfig();

            for (int i = 0; i < 14342; i++)
            {
                Thread.Sleep(2000);
                Console.WriteLine(x.ToJson());

            }
            
            
            Assert.Pass();
        }
    }
}